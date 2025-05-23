﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using UniversityPilot.DAL;

#nullable disable

namespace UniversityPilot.DAL.Migrations
{
    [DbContext(typeof(UniversityPilotContext))]
    [Migration("20250208113523_RemoveManyToManyStudyProgramsSemesters")]
    partial class RemoveManyToManyStudyProgramsSemesters
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ClassDayScheduleClassDay", b =>
                {
                    b.Property<int>("ClassDaysId")
                        .HasColumnType("integer");

                    b.Property<int>("ScheduleClassDaysId")
                        .HasColumnType("integer");

                    b.HasKey("ClassDaysId", "ScheduleClassDaysId");

                    b.HasIndex("ScheduleClassDaysId");

                    b.ToTable("ScheduleClassDayClassDay", (string)null);
                });

            modelBuilder.Entity("CourseDetailsCourseGroup", b =>
                {
                    b.Property<int>("CourseDetailsId")
                        .HasColumnType("integer");

                    b.Property<int>("CourseGroupsId")
                        .HasColumnType("integer");

                    b.HasKey("CourseDetailsId", "CourseGroupsId");

                    b.HasIndex("CourseGroupsId");

                    b.ToTable("CourseGroupCourseDetails", (string)null);
                });

            modelBuilder.Entity("CourseDetailsInstructor", b =>
                {
                    b.Property<int>("CoursesDetailsId")
                        .HasColumnType("integer");

                    b.Property<int>("InstructorsId")
                        .HasColumnType("integer");

                    b.HasKey("CoursesDetailsId", "InstructorsId");

                    b.HasIndex("InstructorsId");

                    b.ToTable("CourseDetailsInstructor", (string)null);
                });

            modelBuilder.Entity("CourseGroupStudent", b =>
                {
                    b.Property<int>("CourseGroupsId")
                        .HasColumnType("integer");

                    b.Property<int>("StudentsId")
                        .HasColumnType("integer");

                    b.HasKey("CourseGroupsId", "StudentsId");

                    b.HasIndex("StudentsId");

                    b.ToTable("StudentCourseGroup", (string)null);
                });

            modelBuilder.Entity("CourseStudyProgram", b =>
                {
                    b.Property<int>("CoursesId")
                        .HasColumnType("integer");

                    b.Property<int>("StudyProgramsId")
                        .HasColumnType("integer");

                    b.HasKey("CoursesId", "StudyProgramsId");

                    b.HasIndex("StudyProgramsId");

                    b.ToTable("StudyProgramCourse", (string)null);
                });

            modelBuilder.Entity("ScheduleClassDayStudyProgram", b =>
                {
                    b.Property<int>("ScheduleClassDaysId")
                        .HasColumnType("integer");

                    b.Property<int>("StudyProgramsId")
                        .HasColumnType("integer");

                    b.HasKey("ScheduleClassDaysId", "StudyProgramsId");

                    b.HasIndex("StudyProgramsId");

                    b.ToTable("ScheduleClassDayStudyProgram", (string)null);
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.AcademicCalendar.Models.Holiday", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.ToTable("Holidays");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.AcademicCalendar.Models.Semester", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AcademicYear")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Semesters");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.Identity.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.Identity.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("EmailIsConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(24)
                        .HasColumnType("character varying(24)");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasDiscriminator().HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.SemesterPlanning.Models.ClassDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("ClassDays");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.SemesterPlanning.Models.CourseGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseType")
                        .HasColumnType("integer");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.ToTable("CourseGroups");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.SemesterPlanning.Models.CourseSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ClassroomId")
                        .HasColumnType("integer");

                    b.Property<int>("CourseGroupId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("InstructorId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("Id");

                    b.HasIndex("ClassroomId");

                    b.HasIndex("CourseGroupId");

                    b.HasIndex("InstructorId");

                    b.ToTable("CourseSchedules");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.SemesterPlanning.Models.ScheduleClassDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("SemesterId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("SemesterId");

                    b.ToTable("ScheduleClassDays");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.SemesterPlanning.Models.SharedCourseGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.ToTable("SharedCourseGroups");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.StudyOrganization.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<int?>("SemesterId")
                        .HasColumnType("integer");

                    b.Property<int>("SemesterNumber")
                        .HasColumnType("integer");

                    b.Property<int?>("SpecializationId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SemesterId");

                    b.HasIndex("SpecializationId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.StudyOrganization.Models.CourseDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AssessmentType")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.Property<int>("CourseType")
                        .HasColumnType("integer");

                    b.Property<int>("ECTS")
                        .HasColumnType("integer");

                    b.Property<int>("Hours")
                        .HasColumnType("integer");

                    b.Property<int?>("SharedCourseGroupId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("SharedCourseGroupId");

                    b.ToTable("CoursesDetails");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.StudyOrganization.Models.FieldOfStudy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.ToTable("FieldsOfStudy");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.StudyOrganization.Models.Specialization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.ToTable("Specializations");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.StudyOrganization.Models.StudyProgram", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("EnrollmentYear")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<int>("FieldOfStudyId")
                        .HasColumnType("integer");

                    b.Property<string>("StudyDegree")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("StudyForm")
                        .HasColumnType("integer");

                    b.Property<bool>("SummerRecruitment")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("FieldOfStudyId");

                    b.ToTable("StudyPrograms");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.UniversityComponents.Models.Classroom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Floor")
                        .HasColumnType("integer");

                    b.Property<string>("RoomNumber")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<int>("SeatCount")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Classrooms");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.UniversityComponents.Models.Instructor", b =>
                {
                    b.HasBaseType("UniversityPilot.DAL.Areas.Identity.Models.User");

                    b.Property<string>("ContractType")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasDiscriminator().HasValue("Instructor");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.UniversityComponents.Models.Student", b =>
                {
                    b.HasBaseType("UniversityPilot.DAL.Areas.Identity.Models.User");

                    b.Property<int>("Indeks")
                        .HasColumnType("integer");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("ClassDayScheduleClassDay", b =>
                {
                    b.HasOne("UniversityPilot.DAL.Areas.SemesterPlanning.Models.ClassDay", null)
                        .WithMany()
                        .HasForeignKey("ClassDaysId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityPilot.DAL.Areas.SemesterPlanning.Models.ScheduleClassDay", null)
                        .WithMany()
                        .HasForeignKey("ScheduleClassDaysId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CourseDetailsCourseGroup", b =>
                {
                    b.HasOne("UniversityPilot.DAL.Areas.StudyOrganization.Models.CourseDetails", null)
                        .WithMany()
                        .HasForeignKey("CourseDetailsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityPilot.DAL.Areas.SemesterPlanning.Models.CourseGroup", null)
                        .WithMany()
                        .HasForeignKey("CourseGroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CourseDetailsInstructor", b =>
                {
                    b.HasOne("UniversityPilot.DAL.Areas.StudyOrganization.Models.CourseDetails", null)
                        .WithMany()
                        .HasForeignKey("CoursesDetailsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityPilot.DAL.Areas.UniversityComponents.Models.Instructor", null)
                        .WithMany()
                        .HasForeignKey("InstructorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CourseGroupStudent", b =>
                {
                    b.HasOne("UniversityPilot.DAL.Areas.SemesterPlanning.Models.CourseGroup", null)
                        .WithMany()
                        .HasForeignKey("CourseGroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityPilot.DAL.Areas.UniversityComponents.Models.Student", null)
                        .WithMany()
                        .HasForeignKey("StudentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CourseStudyProgram", b =>
                {
                    b.HasOne("UniversityPilot.DAL.Areas.StudyOrganization.Models.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityPilot.DAL.Areas.StudyOrganization.Models.StudyProgram", null)
                        .WithMany()
                        .HasForeignKey("StudyProgramsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ScheduleClassDayStudyProgram", b =>
                {
                    b.HasOne("UniversityPilot.DAL.Areas.SemesterPlanning.Models.ScheduleClassDay", null)
                        .WithMany()
                        .HasForeignKey("ScheduleClassDaysId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityPilot.DAL.Areas.StudyOrganization.Models.StudyProgram", null)
                        .WithMany()
                        .HasForeignKey("StudyProgramsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.Identity.Models.User", b =>
                {
                    b.HasOne("UniversityPilot.DAL.Areas.Identity.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.SemesterPlanning.Models.CourseSchedule", b =>
                {
                    b.HasOne("UniversityPilot.DAL.Areas.UniversityComponents.Models.Classroom", "Classroom")
                        .WithMany("CourseSchedules")
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("UniversityPilot.DAL.Areas.SemesterPlanning.Models.CourseGroup", "CourseGroup")
                        .WithMany("CourseSchedules")
                        .HasForeignKey("CourseGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityPilot.DAL.Areas.UniversityComponents.Models.Instructor", "Instructor")
                        .WithMany("CourseSchedules")
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Classroom");

                    b.Navigation("CourseGroup");

                    b.Navigation("Instructor");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.SemesterPlanning.Models.ScheduleClassDay", b =>
                {
                    b.HasOne("UniversityPilot.DAL.Areas.AcademicCalendar.Models.Semester", "Semester")
                        .WithMany("ScheduleClassDays")
                        .HasForeignKey("SemesterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Semester");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.StudyOrganization.Models.Course", b =>
                {
                    b.HasOne("UniversityPilot.DAL.Areas.AcademicCalendar.Models.Semester", "Semester")
                        .WithMany("Courses")
                        .HasForeignKey("SemesterId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("UniversityPilot.DAL.Areas.StudyOrganization.Models.Specialization", "Specialization")
                        .WithMany("Courses")
                        .HasForeignKey("SpecializationId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Semester");

                    b.Navigation("Specialization");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.StudyOrganization.Models.CourseDetails", b =>
                {
                    b.HasOne("UniversityPilot.DAL.Areas.StudyOrganization.Models.Course", "Course")
                        .WithMany("CoursesDetails")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityPilot.DAL.Areas.SemesterPlanning.Models.SharedCourseGroup", "SharedCourseGroup")
                        .WithMany("CoursesDetails")
                        .HasForeignKey("SharedCourseGroupId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Course");

                    b.Navigation("SharedCourseGroup");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.StudyOrganization.Models.StudyProgram", b =>
                {
                    b.HasOne("UniversityPilot.DAL.Areas.StudyOrganization.Models.FieldOfStudy", "FieldOfStudy")
                        .WithMany("Programs")
                        .HasForeignKey("FieldOfStudyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FieldOfStudy");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.AcademicCalendar.Models.Semester", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("ScheduleClassDays");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.Identity.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.SemesterPlanning.Models.CourseGroup", b =>
                {
                    b.Navigation("CourseSchedules");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.SemesterPlanning.Models.SharedCourseGroup", b =>
                {
                    b.Navigation("CoursesDetails");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.StudyOrganization.Models.Course", b =>
                {
                    b.Navigation("CoursesDetails");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.StudyOrganization.Models.FieldOfStudy", b =>
                {
                    b.Navigation("Programs");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.StudyOrganization.Models.Specialization", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.UniversityComponents.Models.Classroom", b =>
                {
                    b.Navigation("CourseSchedules");
                });

            modelBuilder.Entity("UniversityPilot.DAL.Areas.UniversityComponents.Models.Instructor", b =>
                {
                    b.Navigation("CourseSchedules");
                });
#pragma warning restore 612, 618
        }
    }
}
