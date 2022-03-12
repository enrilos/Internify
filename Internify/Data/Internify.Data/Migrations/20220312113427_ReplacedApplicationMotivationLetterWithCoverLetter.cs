using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internify.Data.Migrations
{
    public partial class ReplacedApplicationMotivationLetterWithCoverLetter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MotivationLetter",
                table: "Applications",
                newName: "CoverLetter");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoverLetter",
                table: "Applications",
                newName: "MotivationLetter");
        }
    }
}
