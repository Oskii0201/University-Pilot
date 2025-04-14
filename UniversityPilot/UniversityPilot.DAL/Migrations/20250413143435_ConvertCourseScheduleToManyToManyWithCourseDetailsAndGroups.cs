using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityPilot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ConvertCourseScheduleToManyToManyWithCourseDetailsAndGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedules_CourseGroups_CourseGroupId",
                table: "CourseSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedules_CoursesDetails_CourseDetailsId",
                table: "CourseSchedules");

            migrationBuilder.DropIndex(
                name: "IX_CourseSchedules_CourseDetailsId",
                table: "CourseSchedules");

            migrationBuilder.DropIndex(
                name: "IX_CourseSchedules_CourseGroupId",
                table: "CourseSchedules");

            migrationBuilder.DropColumn(
                name: "CourseDetailsId",
                table: "CourseSchedules");

            migrationBuilder.DropColumn(
                name: "CourseGroupId",
                table: "CourseSchedules");

            migrationBuilder.CreateTable(
                name: "CourseScheduleCourseDetails",
                columns: table => new
                {
                    CourseSchedulesId = table.Column<int>(type: "integer", nullable: false),
                    CoursesDetailsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseScheduleCourseDetails", x => new { x.CourseSchedulesId, x.CoursesDetailsId });
                    table.ForeignKey(
                        name: "FK_CourseScheduleCourseDetails_CourseSchedules_CourseSchedules~",
                        column: x => x.CourseSchedulesId,
                        principalTable: "CourseSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseScheduleCourseDetails_CoursesDetails_CoursesDetailsId",
                        column: x => x.CoursesDetailsId,
                        principalTable: "CoursesDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseScheduleCourseGroups",
                columns: table => new
                {
                    CourseSchedulesId = table.Column<int>(type: "integer", nullable: false),
                    CoursesGroupsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseScheduleCourseGroups", x => new { x.CourseSchedulesId, x.CoursesGroupsId });
                    table.ForeignKey(
                        name: "FK_CourseScheduleCourseGroups_CourseGroups_CoursesGroupsId",
                        column: x => x.CoursesGroupsId,
                        principalTable: "CourseGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseScheduleCourseGroups_CourseSchedules_CourseSchedulesId",
                        column: x => x.CourseSchedulesId,
                        principalTable: "CourseSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseScheduleCourseDetails_CoursesDetailsId",
                table: "CourseScheduleCourseDetails",
                column: "CoursesDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseScheduleCourseGroups_CoursesGroupsId",
                table: "CourseScheduleCourseGroups",
                column: "CoursesGroupsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseScheduleCourseDetails");

            migrationBuilder.DropTable(
                name: "CourseScheduleCourseGroups");

            migrationBuilder.AddColumn<int>(
                name: "CourseDetailsId",
                table: "CourseSchedules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CourseGroupId",
                table: "CourseSchedules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CourseSchedules_CourseDetailsId",
                table: "CourseSchedules",
                column: "CourseDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSchedules_CourseGroupId",
                table: "CourseSchedules",
                column: "CourseGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedules_CourseGroups_CourseGroupId",
                table: "CourseSchedules",
                column: "CourseGroupId",
                principalTable: "CourseGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedules_CoursesDetails_CourseDetailsId",
                table: "CourseSchedules",
                column: "CourseDetailsId",
                principalTable: "CoursesDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
