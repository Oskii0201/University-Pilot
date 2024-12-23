using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityPilot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CourseInstructorToCourseDetailsInstructor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseInstructor");

            migrationBuilder.CreateTable(
                name: "CourseDetailsInstructor",
                columns: table => new
                {
                    CoursesDetailsId = table.Column<int>(type: "integer", nullable: false),
                    InstructorsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDetailsInstructor", x => new { x.CoursesDetailsId, x.InstructorsId });
                    table.ForeignKey(
                        name: "FK_CourseDetailsInstructor_CoursesDetails_CoursesDetailsId",
                        column: x => x.CoursesDetailsId,
                        principalTable: "CoursesDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseDetailsInstructor_Users_InstructorsId",
                        column: x => x.InstructorsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseDetailsInstructor_InstructorsId",
                table: "CourseDetailsInstructor",
                column: "InstructorsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseDetailsInstructor");

            migrationBuilder.CreateTable(
                name: "CourseInstructor",
                columns: table => new
                {
                    CoursesId = table.Column<int>(type: "integer", nullable: false),
                    InstructorsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseInstructor", x => new { x.CoursesId, x.InstructorsId });
                    table.ForeignKey(
                        name: "FK_CourseInstructor_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseInstructor_Users_InstructorsId",
                        column: x => x.InstructorsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseInstructor_InstructorsId",
                table: "CourseInstructor",
                column: "InstructorsId");
        }
    }
}
