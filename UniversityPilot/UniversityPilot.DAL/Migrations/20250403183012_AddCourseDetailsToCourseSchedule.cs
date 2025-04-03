using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityPilot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseDetailsToCourseSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseDetailsId",
                table: "CourseSchedules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CourseSchedules_CourseDetailsId",
                table: "CourseSchedules",
                column: "CourseDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedules_CoursesDetails_CourseDetailsId",
                table: "CourseSchedules",
                column: "CourseDetailsId",
                principalTable: "CoursesDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedules_CoursesDetails_CourseDetailsId",
                table: "CourseSchedules");

            migrationBuilder.DropIndex(
                name: "IX_CourseSchedules_CourseDetailsId",
                table: "CourseSchedules");

            migrationBuilder.DropColumn(
                name: "CourseDetailsId",
                table: "CourseSchedules");
        }
    }
}
