using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using UniversityPilot.DAL.Areas.AcademicCalendar.Models;
using UniversityPilot.DAL.Areas.Identity.Models;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;
using UniversityPilot.DAL.Areas.UniversityComponents.Models;

namespace UniversityPilot.DAL
{
    public class UniversityPilotContext : DbContext
    {
        public UniversityPilotContext(DbContextOptions<UniversityPilotContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;

            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #region DbSet Academic Calendar

        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Semester> Semesters { get; set; }

        #endregion DbSet Academic Calendar

        #region DbSet Identity

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        #endregion DbSet Identity

        #region DbSet Semester Planning

        public DbSet<CourseGroup> CourseGroups { get; set; }
        public DbSet<CourseSchedule> CourseSchedules { get; set; }
        public DbSet<ClassDay> ClassDays { get; set; }
        public DbSet<ScheduleClassDay> ScheduleClassDays { get; set; }
        public DbSet<SharedCourseGroup> sharedCourseGroups { get; set; }

        #endregion DbSet Semester Planning

        #region DbSet Study Organization

        public DbSet<StudyProgram> StudyPrograms { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Specialization> Specializations { get; set; }

        #endregion DbSet Study Organization

        #region DbSet University Components

        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentGroupMembership> StudentGroupsMembership { get; set; }

        #endregion DbSet University Components

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region AcademicCalendar Configuration

            modelBuilder.Entity<Holiday>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(64);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Description).IsRequired(false).HasMaxLength(256);
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AcademicYear).IsRequired().HasMaxLength(16);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(64);
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();

                entity.HasMany(e => e.Courses)
                      .WithOne(c => c.Semester)
                      .HasForeignKey(c => c.SemesterId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(e => e.ScheduleClassDays)
                      .WithOne(scd => scd.Semester)
                      .HasForeignKey(scd => scd.SemesterId);

                entity.HasMany(e => e.StudyPrograms)
                      .WithMany(sp => sp.Semesters)
                      .UsingEntity(j => j.ToTable("StudyProgramSemester"));
            });

            #endregion AcademicCalendar Configuration

            #region Identity Configuration

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.EmailIsConfirmed)
                      .HasDefaultValue(false)
                      .IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.PhoneNumber)
                      .HasMaxLength(24)
                      .IsRequired(false);
                entity.HasOne(e => e.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(e => e.RoleId);
                entity.HasDiscriminator<string>("Discriminator")
                      .HasValue<User>("User")
                      .HasValue<Student>("Student")
                      .HasValue<Instructor>("Instructor");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.HasMany(e => e.Users)
                      .WithOne(u => u.Role)
                      .HasForeignKey(u => u.RoleId);
            });

            #endregion Identity Configuration

            #region Semester Planning Configuration

            modelBuilder.Entity<ClassDay>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StartDateTime).IsRequired();
                entity.Property(e => e.EndDateTime).IsRequired();

                entity.HasMany(e => e.ScheduleClassDays)
                      .WithMany(scd => scd.ClassDays)
                      .UsingEntity(j => j.ToTable("ScheduleClassDayClassDay"));
            });

            modelBuilder.Entity<ScheduleClassDay>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(256);

                entity.HasOne(e => e.Semester)
                      .WithMany(s => s.ScheduleClassDays)
                      .HasForeignKey(e => e.SemesterId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.StudyPrograms)
                      .WithMany(sp => sp.ScheduleClassDays)
                      .UsingEntity(j => j.ToTable("ScheduleClassDayStudyProgram"));

                entity.HasMany(e => e.ClassDays)
                      .WithMany(cd => cd.ScheduleClassDays)
                      .UsingEntity(j => j.ToTable("ScheduleClassDayClassDay"));

                entity.HasIndex(e => e.SemesterId);
            });

            modelBuilder.Entity<SharedCourseGroup>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(128);

                entity.HasMany(e => e.Courses)
                      .WithOne(c => c.SharedCourseGroup)
                      .HasForeignKey(c => c.SharedCourseGroupId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            #endregion Semester Planning Configuration

            #region Study Organization Configuration

            modelBuilder.Entity<StudyProgram>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AcademicYear).IsRequired().HasMaxLength(16);
                entity.Property(e => e.StudyDegree).IsRequired().HasMaxLength(64);
                entity.Property(e => e.FieldOfStudy).IsRequired().HasMaxLength(128);
                entity.Property(e => e.StudyForm).IsRequired();
                entity.Property(e => e.SummerRecruitment).IsRequired();

                entity.HasMany(e => e.Specializations)
                      .WithMany(s => s.StudyPrograms)
                      .UsingEntity(j => j.ToTable("StudyProgramSpecialization"));

                entity.HasMany(e => e.Courses)
                      .WithMany(c => c.StudyProgram)
                      .UsingEntity(j => j.ToTable("StudyProgramCourse"));

                entity.HasMany(e => e.ScheduleClassDays)
                      .WithMany(scd => scd.StudyPrograms)
                      .UsingEntity(j => j.ToTable("ScheduleClassDayStudyProgram"));

                entity.HasMany(e => e.Semesters)
                      .WithMany(s => s.StudyPrograms)
                      .UsingEntity(j => j.ToTable("StudyProgramSemester"));
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(128);
                entity.Property(e => e.SemesterNumber).IsRequired();
                entity.Property(e => e.CourseType).IsRequired();
                entity.Property(e => e.Hours).IsRequired();
                entity.Property(e => e.AssessmentType).IsRequired().HasMaxLength(64);
                entity.Property(e => e.ECTS).IsRequired();

                entity.HasOne(e => e.Semester)
                      .WithMany(s => s.Courses)
                      .HasForeignKey(e => e.SemesterId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Specialization)
                      .WithMany(s => s.Courses)
                      .HasForeignKey(e => e.SpecializationId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(e => e.StudyProgram)
                      .WithMany(sp => sp.Courses)
                      .UsingEntity(j => j.ToTable("StudyProgramCourse"));

                entity.HasOne(e => e.SharedCourseGroup)
                      .WithMany(scg => scg.Courses)
                      .HasForeignKey(e => e.SharedCourseGroupId);

                entity.HasIndex(e => e.SemesterId);
            });

            modelBuilder.Entity<Specialization>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(128);

                entity.HasMany(e => e.StudyPrograms)
                      .WithMany(sp => sp.Specializations)
                      .UsingEntity(j => j.ToTable("StudyProgramSpecialization"));

                entity.HasMany(e => e.Courses)
                      .WithOne(c => c.Specialization)
                      .HasForeignKey(c => c.SpecializationId);
            });

            #endregion Study Organization Configuration

            #region University and Scheduling Configuration

            modelBuilder.Entity<Classroom>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RoomNumber).IsRequired().HasMaxLength(32);
                entity.Property(e => e.Floor).IsRequired();
                entity.Property(e => e.SeatCount).IsRequired();
                entity.HasMany(e => e.CourseSchedules)
                      .WithOne(cs => cs.Classroom)
                      .HasForeignKey(cs => cs.ClassroomId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<CourseGroup>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.GroupName).IsRequired().HasMaxLength(64);
                entity.HasMany(e => e.CourseSchedules)
                      .WithOne(cs => cs.CourseGroup)
                      .HasForeignKey(cs => cs.CourseGroupId);
                entity.HasMany(e => e.Students)
                      .WithMany(s => s.CourseGroups)
                      .UsingEntity(j => j.ToTable("StudentCourseGroup"));
            });

            modelBuilder.Entity<CourseSchedule>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StartDateTime).IsRequired();
                entity.Property(e => e.EndDateTime).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasMaxLength(32);
                entity.HasOne(e => e.CourseGroup)
                      .WithMany(cg => cg.CourseSchedules)
                      .HasForeignKey(e => e.CourseGroupId);
                entity.HasOne(e => e.Classroom)
                      .WithMany(c => c.CourseSchedules)
                      .HasForeignKey(e => e.ClassroomId);
                entity.HasOne(e => e.Instructor)
                      .WithMany(i => i.CourseSchedules)
                      .HasForeignKey(e => e.InstructorId);
            });

            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.Property(e => e.ContractType).IsRequired().HasMaxLength(64);
                entity.HasMany(e => e.CourseSchedules)
                      .WithOne(cs => cs.Instructor)
                      .HasForeignKey(cs => cs.InstructorId);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.Indeks).IsRequired();
                entity.HasMany(e => e.CourseGroups)
                      .WithMany(cg => cg.Students)
                      .UsingEntity(j => j.ToTable("StudentCourseGroup"));
            });

            #endregion University and Scheduling Configuration
        }
    }
}