using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityPilot.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStudyDegreeOnEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudyDegreeTemp",
                table: "StudyPrograms",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.Sql(@"
                UPDATE ""StudyPrograms""
                SET ""StudyDegreeTemp"" = CASE ""StudyDegree""
                    WHEN 'inż.' THEN 0
                    WHEN 'Lic' THEN 1
                    WHEN 'mgr' THEN 2
                    WHEN 'USM' THEN 3
                    WHEN 'USM 3sem' THEN 4
                    WHEN 'USM  3sem' THEN 4
                    WHEN 'USM + SP' THEN 5
                    ELSE 6
                END
            ");

            migrationBuilder.DropColumn(
                name: "StudyDegree",
                table: "StudyPrograms"
            );

            migrationBuilder.RenameColumn(
                name: "StudyDegreeTemp",
                table: "StudyPrograms",
                newName: "StudyDegree"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudyDegreeOld",
                table: "StudyPrograms",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.Sql(@"
                UPDATE ""StudyPrograms""
                SET ""StudyDegreeOld"" = CASE ""StudyDegree""
                    WHEN 0 THEN 'inż.'
                    WHEN 1 THEN 'Lic'
                    WHEN 2 THEN 'mgr'
                    WHEN 3 THEN 'USM'
                    WHEN 4 THEN 'USM 3sem'
                    WHEN 5 THEN 'USM + SP'
                    ELSE 'Nieznany'
                END
            ");

            migrationBuilder.DropColumn(
                name: "StudyDegree",
                table: "StudyPrograms"
            );

            migrationBuilder.RenameColumn(
                name: "StudyDegreeOld",
                table: "StudyPrograms",
                newName: "StudyDegree"
            );
        }
    }
}
