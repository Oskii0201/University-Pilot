using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UniversityPilot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MoveFieldOfStudyToSeparateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FieldOfStudy",
                table: "StudyPrograms");

            migrationBuilder.AddColumn<int>(
                name: "FieldOfStudyId",
                table: "StudyPrograms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FieldsOfStudy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldsOfStudy", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudyPrograms_FieldOfStudyId",
                table: "StudyPrograms",
                column: "FieldOfStudyId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyPrograms_FieldsOfStudy_FieldOfStudyId",
                table: "StudyPrograms",
                column: "FieldOfStudyId",
                principalTable: "FieldsOfStudy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyPrograms_FieldsOfStudy_FieldOfStudyId",
                table: "StudyPrograms");

            migrationBuilder.DropTable(
                name: "FieldsOfStudy");

            migrationBuilder.DropIndex(
                name: "IX_StudyPrograms_FieldOfStudyId",
                table: "StudyPrograms");

            migrationBuilder.DropColumn(
                name: "FieldOfStudyId",
                table: "StudyPrograms");

            migrationBuilder.AddColumn<string>(
                name: "FieldOfStudy",
                table: "StudyPrograms",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }
    }
}
