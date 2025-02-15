using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityPilot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSpecializationFromStudyProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudyProgramSpecialization");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_StudyProgramSpecialization_StudyProgramsId",
                table: "StudyProgramSpecialization",
                column: "StudyProgramsId");
        }
    }
}
