using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internify.Data.Migrations
{
    public partial class ConvertedInternshipCountryIdPropertyToNonUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Internships_CountryId",
                table: "Internships");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_CountryId",
                table: "Internships",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Internships_CountryId",
                table: "Internships");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_CountryId",
                table: "Internships",
                column: "CountryId",
                unique: true,
                filter: "[CountryId] IS NOT NULL");
        }
    }
}
