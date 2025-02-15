using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityPilot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveManyToManyStudyProgramsSemesters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudyProgramSemester");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_StudyProgramSemester_StudyProgramsId",
                table: "StudyProgramSemester",
                column: "StudyProgramsId");
        }
    }
}
