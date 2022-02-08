using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internify.Data.Migrations
{
    public partial class RenamedTheIdsOfEachModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UniversityId",
                table: "Universities",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SpecializationId",
                table: "Specializations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ReviewId",
                table: "Reviews",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "InternshipId",
                table: "Internships",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Countries",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Companies",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CandidateId",
                table: "Candidates",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "Articles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "Applications",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Universities",
                newName: "UniversityId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Specializations",
                newName: "SpecializationId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Reviews",
                newName: "ReviewId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Internships",
                newName: "InternshipId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Countries",
                newName: "CountryId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Companies",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Candidates",
                newName: "CandidateId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Articles",
                newName: "ArticleId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Applications",
                newName: "ApplicationId");
        }
    }
}
