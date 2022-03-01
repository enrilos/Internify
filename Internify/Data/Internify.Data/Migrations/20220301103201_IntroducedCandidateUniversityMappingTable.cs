using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internify.Data.Migrations
{
    public partial class IntroducedCandidateUniversityMappingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_Universities_UniversityId",
                table: "Candidates");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_UniversityId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "Candidates");

            migrationBuilder.CreateTable(
                name: "CandidateUniversities",
                columns: table => new
                {
                    CandidateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UniversityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateUniversities", x => new { x.CandidateId, x.UniversityId });
                    table.ForeignKey(
                        name: "FK_CandidateUniversities_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CandidateUniversities_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidateUniversities_UniversityId",
                table: "CandidateUniversities",
                column: "UniversityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidateUniversities");

            migrationBuilder.AddColumn<string>(
                name: "UniversityId",
                table: "Candidates",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_UniversityId",
                table: "Candidates",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_Universities_UniversityId",
                table: "Candidates",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
