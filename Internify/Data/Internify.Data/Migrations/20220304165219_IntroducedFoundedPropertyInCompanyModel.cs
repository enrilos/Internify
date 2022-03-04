using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internify.Data.Migrations
{
    public partial class IntroducedFoundedPropertyInCompanyModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Founded",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Founded",
                table: "Companies");
        }
    }
}
