using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using UniversityPilot.DAL.Areas.Identity.Models;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;
using UniversityPilot.DAL.Areas.UniversityAndScheduling.Models;

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

        #region DbSet Identity

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        #endregion DbSet Identity

        #region DbSet Study Organization

        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseType> CourseTypes { get; set; }
        public DbSet<FieldOfStudy> FieldsOfStudy { get; set; }
        public DbSet<Specialization> Specializations { get; set; }

        #endregion DbSet Study Organization

        #region DbSet University and Scheduling

        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<CourseGroup> CourseGroups { get; set; }
        public DbSet<CourseSchedule> CourseSchedules { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<StudySchedule> StudySchedules { get; set; }

        #endregion DbSet University and Scheduling

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
                      .HasForeignKey(e => e.RoleID);
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
                      .HasForeignKey(u => u.RoleID);
            });

            #endregion Identity Configuration

            #region Study Organization Configuration

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CourseName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CourseCode).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Credits).IsRequired();
                entity.Property(e => e.CourseDuration).IsRequired();
                entity.Property(e => e.Online).IsRequired();
                entity.HasOne(e => e.CourseType)
                      .WithMany(ct => ct.Courses)
                      .HasForeignKey(e => e.CourseTypeId);
                entity.HasMany(e => e.CourseGroups)
                      .WithMany(c => c.Courses)
                      .UsingEntity(j => j.ToTable("CourseGroupCourse"));
                entity.HasMany(e => e.FieldOfStudies)
                      .WithMany(fs => fs.Courses)
                      .UsingEntity(j => j.ToTable("CourseFieldOfStudy"));
                entity.HasMany(e => e.Specializations)
                      .WithMany(s => s.Courses)
                      .UsingEntity(j => j.ToTable("CourseSpecialization"));
                entity.HasMany(e => e.Instructors)
                      .WithMany(i => i.Courses)
                      .UsingEntity(j => j.ToTable("InstructorCourse"));
            });

            modelBuilder.Entity<CourseType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.HasMany(e => e.Courses)
                      .WithOne(c => c.CourseType)
                      .HasForeignKey(c => c.CourseTypeId);
            });

            modelBuilder.Entity<FieldOfStudy>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FormOfStudy).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LevelOfEducation).IsRequired().HasMaxLength(50);
                entity.HasMany(e => e.Specializations)
                      .WithMany(s => s.FieldOfStudies)
                      .UsingEntity(j => j.ToTable("SpecializationFieldOfStudy"));
                entity.HasMany(e => e.Students)
                      .WithMany(s => s.FieldOfStudies)
                      .UsingEntity(j => j.ToTable("StudentFieldOfStudy"));
                entity.HasMany(e => e.Courses)
                      .WithMany(c => c.FieldOfStudies)
                      .UsingEntity(j => j.ToTable("CourseFieldOfStudy"));
                entity.HasMany(e => e.StudySchedules)
                      .WithMany(ss => ss.FieldOfStudies)
                      .UsingEntity(j => j.ToTable("FieldOfStudySchedules"));
            });

            modelBuilder.Entity<Specialization>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasMany(e => e.FieldOfStudies)
                      .WithMany(fs => fs.Specializations)
                      .UsingEntity(j => j.ToTable("SpecializationFieldOfStudy"));
                entity.HasMany(e => e.Students)
                      .WithMany(s => s.Specializations)
                      .UsingEntity(j => j.ToTable("StudentSpecialization"));
                entity.HasMany(e => e.Courses)
                      .WithMany(c => c.Specializations)
                      .UsingEntity(j => j.ToTable("CourseSpecialization"));
            });

            #endregion Study Organization Configuration

            #region University and Scheduling Configuration

            modelBuilder.Entity<Classroom>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RoomNumber).IsRequired().HasMaxLength(20);
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
                entity.Property(e => e.GroupName).IsRequired().HasMaxLength(50);
                entity.HasMany(e => e.CourseSchedules)
                      .WithOne(cs => cs.CourseGroup)
                      .HasForeignKey(cs => cs.CourseGroupId);
                entity.HasMany(e => e.Courses)
                      .WithMany(c => c.CourseGroups)
                      .UsingEntity(j => j.ToTable("CourseGroupCourse"));
                entity.HasMany(e => e.Students)
                      .WithMany(s => s.CourseGroups)
                      .UsingEntity(j => j.ToTable("StudentCourseGroup"));
            });

            modelBuilder.Entity<CourseSchedule>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StartDateTime).IsRequired();
                entity.Property(e => e.EndDateTime).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
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

            modelBuilder.Entity<Holiday>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Description).IsRequired(false).HasMaxLength(200);
            });

            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.Property(e => e.ContractType).IsRequired().HasMaxLength(50);
                entity.HasMany(e => e.CourseSchedules)
                      .WithOne(cs => cs.Instructor)
                      .HasForeignKey(cs => cs.InstructorId);
                entity.HasMany(e => e.Courses)
                      .WithMany(c => c.Instructors)
                      .UsingEntity(j => j.ToTable("InstructorCourse"));
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.Indeks).IsRequired();
                entity.HasMany(e => e.CourseGroups)
                      .WithMany(cg => cg.Students)
                      .UsingEntity(j => j.ToTable("StudentCourseGroup"));
                entity.HasMany(e => e.FieldOfStudies)
                      .WithMany(fs => fs.Students)
                      .UsingEntity(j => j.ToTable("StudentFieldOfStudy"));
                entity.HasMany(e => e.Specializations)
                      .WithMany(s => s.Students)
                      .UsingEntity(j => j.ToTable("StudentSpecialization"));
            });

            modelBuilder.Entity<StudySchedule>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();
                entity.HasMany(e => e.FieldOfStudies)
                      .WithMany(fs => fs.StudySchedules)
                      .UsingEntity(j => j.ToTable("FieldOfStudySchedules"));
            });

            #endregion University and Scheduling Configuration
        }
    }
}