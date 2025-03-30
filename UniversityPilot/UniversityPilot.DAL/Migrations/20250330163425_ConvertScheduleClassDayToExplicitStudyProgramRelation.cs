using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityPilot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ConvertScheduleClassDayToExplicitStudyProgramRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleClassDayStudyProgram_StudyPrograms_StudyProgramsId",
                table: "ScheduleClassDayStudyProgram");

            migrationBuilder.RenameColumn(
                name: "StudyProgramsId",
                table: "ScheduleClassDayStudyProgram",
                newName: "StudyProgramId");

            migrationBuilder.RenameColumn(
                name: "ScheduleClassDaysId",
                table: "ScheduleClassDayStudyProgram",
                newName: "ScheduleClassDayId");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleClassDayStudyProgram_StudyProgramsId",
                table: "ScheduleClassDayStudyProgram",
                newName: "IX_ScheduleClassDayStudyProgram_StudyProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleClassDayStudyProgram_StudyPrograms_StudyProgramId",
                table: "ScheduleClassDayStudyProgram",
                column: "StudyProgramId",
                principalTable: "StudyPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleClassDayStudyProgram_StudyPrograms_StudyProgramId",
                table: "ScheduleClassDayStudyProgram");

            migrationBuilder.RenameColumn(
                name: "StudyProgramId",
                table: "ScheduleClassDayStudyProgram",
                newName: "StudyProgramsId");

            migrationBuilder.RenameColumn(
                name: "ScheduleClassDayId",
                table: "ScheduleClassDayStudyProgram",
                newName: "ScheduleClassDaysId");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleClassDayStudyProgram_StudyProgramId",
                table: "ScheduleClassDayStudyProgram",
                newName: "IX_ScheduleClassDayStudyProgram_StudyProgramsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleClassDayStudyProgram_StudyPrograms_StudyProgramsId",
                table: "ScheduleClassDayStudyProgram",
                column: "StudyProgramsId",
                principalTable: "StudyPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
