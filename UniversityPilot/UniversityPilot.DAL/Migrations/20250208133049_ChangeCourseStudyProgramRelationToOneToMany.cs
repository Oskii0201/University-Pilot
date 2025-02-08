using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityPilot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCourseStudyProgramRelationToOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudyProgramCourse");

            migrationBuilder.AddColumn<int>(
                name: "StudyProgramId",
                table: "Courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_StudyProgramId",
                table: "Courses",
                column: "StudyProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_StudyPrograms_StudyProgramId",
                table: "Courses",
                column: "StudyProgramId",
                principalTable: "StudyPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_StudyPrograms_StudyProgramId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_StudyProgramId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "StudyProgramId",
                table: "Courses");

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

            migrationBuilder.CreateIndex(
                name: "IX_StudyProgramCourse_StudyProgramsId",
                table: "StudyProgramCourse",
                column: "StudyProgramsId");
        }
    }
}
