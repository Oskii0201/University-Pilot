using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UniversityPilot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FinalDatabaseRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseTypes_CourseTypeId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorCourse_Courses_CoursesId",
                table: "InstructorCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorCourse_Users_InstructorsId",
                table: "InstructorCourse");

            migrationBuilder.DropTable(
                name: "CourseFieldOfStudy");

            migrationBuilder.DropTable(
                name: "CourseGroupCourse");

            migrationBuilder.DropTable(
                name: "CourseSpecialization");

            migrationBuilder.DropTable(
                name: "CourseTypes");

            migrationBuilder.DropTable(
                name: "FieldOfStudySchedules");

            migrationBuilder.DropTable(
                name: "SpecializationFieldOfStudy");

            migrationBuilder.DropTable(
                name: "StudentFieldOfStudy");

            migrationBuilder.DropTable(
                name: "StudentSpecialization");

            migrationBuilder.DropTable(
                name: "StudySchedules");

            migrationBuilder.DropTable(
                name: "FieldsOfStudy");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseTypeId",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstructorCourse",
                table: "InstructorCourse");

            migrationBuilder.DropColumn(
                name: "CourseCode",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseDuration",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseTypeId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Online",
                table: "Courses");

            migrationBuilder.RenameTable(
                name: "InstructorCourse",
                newName: "CourseInstructor");

            migrationBuilder.RenameColumn(
                name: "Credits",
                table: "Courses",
                newName: "SemesterNumber");

            migrationBuilder.RenameIndex(
                name: "IX_InstructorCourse_InstructorsId",
                table: "CourseInstructor",
                newName: "IX_CourseInstructor_InstructorsId");

            migrationBuilder.AlterColumn<string>(
                name: "ContractType",
                table: "Users",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Specializations",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Holidays",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Holidays",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "CourseSchedules",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Courses",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SemesterId",
                table: "Courses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecializationId",
                table: "Courses",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                table: "CourseGroups",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "CourseType",
                table: "CourseGroups",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "RoomNumber",
                table: "Classrooms",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseInstructor",
                table: "CourseInstructor",
                columns: new[] { "CoursesId", "InstructorsId" });

            migrationBuilder.CreateTable(
                name: "ClassDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassDays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AcademicYear = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semesters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SharedCourseGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedCourseGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudyPrograms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AcademicYear = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    StudyDegree = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    FieldOfStudy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    StudyForm = table.Column<int>(type: "integer", nullable: false),
                    SummerRecruitment = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPrograms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleClassDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SemesterId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleClassDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleClassDays_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoursesDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CourseType = table.Column<int>(type: "integer", nullable: false),
                    Hours = table.Column<int>(type: "integer", nullable: false),
                    AssessmentType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ECTS = table.Column<int>(type: "integer", nullable: false),
                    CourseId = table.Column<int>(type: "integer", nullable: false),
                    SharedCourseGroupId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursesDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoursesDetails_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoursesDetails_SharedCourseGroups_SharedCourseGroupId",
                        column: x => x.SharedCourseGroupId,
                        principalTable: "SharedCourseGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "StudyProgramCourse",
                columns: table => new
                {
                    CoursesId = table.Column<int>(type: "integer", nullable: false),
                    StudyProgramsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyProgramCourse", x => new { x.CoursesId, x.StudyProgramsId });
                    table.ForeignKey(
                        name: "FK_StudyProgramCourse_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudyProgramCourse_StudyPrograms_StudyProgramsId",
                        column: x => x.StudyProgramsId,
                        principalTable: "StudyPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudyProgramSemester",
                columns: table => new
                {
                    SemestersId = table.Column<int>(type: "integer", nullable: false),
                    StudyProgramsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyProgramSemester", x => new { x.SemestersId, x.StudyProgramsId });
                    table.ForeignKey(
                        name: "FK_StudyProgramSemester_Semesters_SemestersId",
                        column: x => x.SemestersId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudyProgramSemester_StudyPrograms_StudyProgramsId",
                        column: x => x.StudyProgramsId,
                        principalTable: "StudyPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudyProgramSpecialization",
                columns: table => new
                {
                    SpecializationsId = table.Column<int>(type: "integer", nullable: false),
                    StudyProgramsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyProgramSpecialization", x => new { x.SpecializationsId, x.StudyProgramsId });
                    table.ForeignKey(
                        name: "FK_StudyProgramSpecialization_Specializations_SpecializationsId",
                        column: x => x.SpecializationsId,
                        principalTable: "Specializations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudyProgramSpecialization_StudyPrograms_StudyProgramsId",
                        column: x => x.StudyProgramsId,
                        principalTable: "StudyPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleClassDayClassDay",
                columns: table => new
                {
                    ClassDaysId = table.Column<int>(type: "integer", nullable: false),
                    ScheduleClassDaysId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleClassDayClassDay", x => new { x.ClassDaysId, x.ScheduleClassDaysId });
                    table.ForeignKey(
                        name: "FK_ScheduleClassDayClassDay_ClassDays_ClassDaysId",
                        column: x => x.ClassDaysId,
                        principalTable: "ClassDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleClassDayClassDay_ScheduleClassDays_ScheduleClassDay~",
                        column: x => x.ScheduleClassDaysId,
                        principalTable: "ScheduleClassDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleClassDayStudyProgram",
                columns: table => new
                {
                    ScheduleClassDaysId = table.Column<int>(type: "integer", nullable: false),
                    StudyProgramsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleClassDayStudyProgram", x => new { x.ScheduleClassDaysId, x.StudyProgramsId });
                    table.ForeignKey(
                        name: "FK_ScheduleClassDayStudyProgram_ScheduleClassDays_ScheduleClas~",
                        column: x => x.ScheduleClassDaysId,
                        principalTable: "ScheduleClassDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleClassDayStudyProgram_StudyPrograms_StudyProgramsId",
                        column: x => x.StudyProgramsId,
                        principalTable: "StudyPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseGroupCourseDetails",
                columns: table => new
                {
                    CourseDetailsId = table.Column<int>(type: "integer", nullable: false),
                    CourseGroupsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseGroupCourseDetails", x => new { x.CourseDetailsId, x.CourseGroupsId });
                    table.ForeignKey(
                        name: "FK_CourseGroupCourseDetails_CourseGroups_CourseGroupsId",
                        column: x => x.CourseGroupsId,
                        principalTable: "CourseGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseGroupCourseDetails_CoursesDetails_CourseDetailsId",
                        column: x => x.CourseDetailsId,
                        principalTable: "CoursesDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SemesterId",
                table: "Courses",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SpecializationId",
                table: "Courses",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseGroupCourseDetails_CourseGroupsId",
                table: "CourseGroupCourseDetails",
                column: "CourseGroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursesDetails_CourseId",
                table: "CoursesDetails",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursesDetails_SharedCourseGroupId",
                table: "CoursesDetails",
                column: "SharedCourseGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleClassDayClassDay_ScheduleClassDaysId",
                table: "ScheduleClassDayClassDay",
                column: "ScheduleClassDaysId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleClassDays_SemesterId",
                table: "ScheduleClassDays",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleClassDayStudyProgram_StudyProgramsId",
                table: "ScheduleClassDayStudyProgram",
                column: "StudyProgramsId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyProgramCourse_StudyProgramsId",
                table: "StudyProgramCourse",
                column: "StudyProgramsId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyProgramSemester_StudyProgramsId",
                table: "StudyProgramSemester",
                column: "StudyProgramsId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyProgramSpecialization_StudyProgramsId",
                table: "StudyProgramSpecialization",
                column: "StudyProgramsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseInstructor_Courses_CoursesId",
                table: "CourseInstructor",
                column: "CoursesId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseInstructor_Users_InstructorsId",
                table: "CourseInstructor",
                column: "InstructorsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Semesters_SemesterId",
                table: "Courses",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Specializations_SpecializationId",
                table: "Courses",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseInstructor_Courses_CoursesId",
                table: "CourseInstructor");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseInstructor_Users_InstructorsId",
                table: "CourseInstructor");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Semesters_SemesterId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Specializations_SpecializationId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "CourseGroupCourseDetails");

            migrationBuilder.DropTable(
                name: "ScheduleClassDayClassDay");

            migrationBuilder.DropTable(
                name: "ScheduleClassDayStudyProgram");

            migrationBuilder.DropTable(
                name: "StudyProgramCourse");

            migrationBuilder.DropTable(
                name: "StudyProgramSemester");

            migrationBuilder.DropTable(
                name: "StudyProgramSpecialization");

            migrationBuilder.DropTable(
                name: "CoursesDetails");

            migrationBuilder.DropTable(
                name: "ClassDays");

            migrationBuilder.DropTable(
                name: "ScheduleClassDays");

            migrationBuilder.DropTable(
                name: "StudyPrograms");

            migrationBuilder.DropTable(
                name: "SharedCourseGroups");

            migrationBuilder.DropTable(
                name: "Semesters");

            migrationBuilder.DropIndex(
                name: "IX_Courses_SemesterId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_SpecializationId",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseInstructor",
                table: "CourseInstructor");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "SemesterId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "SpecializationId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseType",
                table: "CourseGroups");

            migrationBuilder.RenameTable(
                name: "CourseInstructor",
                newName: "InstructorCourse");

            migrationBuilder.RenameColumn(
                name: "SemesterNumber",
                table: "Courses",
                newName: "Credits");

            migrationBuilder.RenameIndex(
                name: "IX_CourseInstructor_InstructorsId",
                table: "InstructorCourse",
                newName: "IX_InstructorCourse_InstructorsId");

            migrationBuilder.AlterColumn<string>(
                name: "ContractType",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Specializations",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Holidays",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Holidays",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "CourseSchedules",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<string>(
                name: "CourseCode",
                table: "Courses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "CourseDuration",
                table: "Courses",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "CourseName",
                table: "Courses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CourseTypeId",
                table: "Courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Online",
                table: "Courses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                table: "CourseGroups",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "RoomNumber",
                table: "Classrooms",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstructorCourse",
                table: "InstructorCourse",
                columns: new[] { "CoursesId", "InstructorsId" });

            migrationBuilder.CreateTable(
                name: "CourseGroupCourse",
                columns: table => new
                {
                    CourseGroupsId = table.Column<int>(type: "integer", nullable: false),
                    CoursesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseGroupCourse", x => new { x.CourseGroupsId, x.CoursesId });
                    table.ForeignKey(
                        name: "FK_CourseGroupCourse_CourseGroups_CourseGroupsId",
                        column: x => x.CourseGroupsId,
                        principalTable: "CourseGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseGroupCourse_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseSpecialization",
                columns: table => new
                {
                    CoursesId = table.Column<int>(type: "integer", nullable: false),
                    SpecializationsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSpecialization", x => new { x.CoursesId, x.SpecializationsId });
                    table.ForeignKey(
                        name: "FK_CourseSpecialization_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseSpecialization_Specializations_SpecializationsId",
                        column: x => x.SpecializationsId,
                        principalTable: "Specializations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldsOfStudy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FormOfStudy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LevelOfEducation = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldsOfStudy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentSpecialization",
                columns: table => new
                {
                    SpecializationsId = table.Column<int>(type: "integer", nullable: false),
                    StudentsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentSpecialization", x => new { x.SpecializationsId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_StudentSpecialization_Specializations_SpecializationsId",
                        column: x => x.SpecializationsId,
                        principalTable: "Specializations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentSpecialization_Users_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudySchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudySchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseFieldOfStudy",
                columns: table => new
                {
                    CoursesId = table.Column<int>(type: "integer", nullable: false),
                    FieldOfStudiesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseFieldOfStudy", x => new { x.CoursesId, x.FieldOfStudiesId });
                    table.ForeignKey(
                        name: "FK_CourseFieldOfStudy_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseFieldOfStudy_FieldsOfStudy_FieldOfStudiesId",
                        column: x => x.FieldOfStudiesId,
                        principalTable: "FieldsOfStudy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecializationFieldOfStudy",
                columns: table => new
                {
                    FieldOfStudiesId = table.Column<int>(type: "integer", nullable: false),
                    SpecializationsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecializationFieldOfStudy", x => new { x.FieldOfStudiesId, x.SpecializationsId });
                    table.ForeignKey(
                        name: "FK_SpecializationFieldOfStudy_FieldsOfStudy_FieldOfStudiesId",
                        column: x => x.FieldOfStudiesId,
                        principalTable: "FieldsOfStudy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecializationFieldOfStudy_Specializations_SpecializationsId",
                        column: x => x.SpecializationsId,
                        principalTable: "Specializations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentFieldOfStudy",
                columns: table => new
                {
                    FieldOfStudiesId = table.Column<int>(type: "integer", nullable: false),
                    StudentsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentFieldOfStudy", x => new { x.FieldOfStudiesId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_StudentFieldOfStudy_FieldsOfStudy_FieldOfStudiesId",
                        column: x => x.FieldOfStudiesId,
                        principalTable: "FieldsOfStudy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentFieldOfStudy_Users_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldOfStudySchedules",
                columns: table => new
                {
                    FieldOfStudiesId = table.Column<int>(type: "integer", nullable: false),
                    StudySchedulesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldOfStudySchedules", x => new { x.FieldOfStudiesId, x.StudySchedulesId });
                    table.ForeignKey(
                        name: "FK_FieldOfStudySchedules_FieldsOfStudy_FieldOfStudiesId",
                        column: x => x.FieldOfStudiesId,
                        principalTable: "FieldsOfStudy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldOfStudySchedules_StudySchedules_StudySchedulesId",
                        column: x => x.StudySchedulesId,
                        principalTable: "StudySchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseTypeId",
                table: "Courses",
                column: "CourseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseFieldOfStudy_FieldOfStudiesId",
                table: "CourseFieldOfStudy",
                column: "FieldOfStudiesId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseGroupCourse_CoursesId",
                table: "CourseGroupCourse",
                column: "CoursesId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSpecialization_SpecializationsId",
                table: "CourseSpecialization",
                column: "SpecializationsId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldOfStudySchedules_StudySchedulesId",
                table: "FieldOfStudySchedules",
                column: "StudySchedulesId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecializationFieldOfStudy_SpecializationsId",
                table: "SpecializationFieldOfStudy",
                column: "SpecializationsId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFieldOfStudy_StudentsId",
                table: "StudentFieldOfStudy",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSpecialization_StudentsId",
                table: "StudentSpecialization",
                column: "StudentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseTypes_CourseTypeId",
                table: "Courses",
                column: "CourseTypeId",
                principalTable: "CourseTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorCourse_Courses_CoursesId",
                table: "InstructorCourse",
                column: "CoursesId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorCourse_Users_InstructorsId",
                table: "InstructorCourse",
                column: "InstructorsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
