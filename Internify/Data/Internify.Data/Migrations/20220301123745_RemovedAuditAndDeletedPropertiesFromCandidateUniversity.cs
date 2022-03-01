using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internify.Data.Migrations
{
    public partial class RemovedAuditAndDeletedPropertiesFromCandidateUniversity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateUniversities_Candidates_CandidateId",
                table: "CandidateUniversities");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidateUniversities_Universities_UniversityId",
                table: "CandidateUniversities");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "CandidateUniversities");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "CandidateUniversities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CandidateUniversities");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "CandidateUniversities");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateUniversities_Candidates_CandidateId",
                table: "CandidateUniversities",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateUniversities_Universities_UniversityId",
                table: "CandidateUniversities",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateUniversities_Candidates_CandidateId",
                table: "CandidateUniversities");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidateUniversities_Universities_UniversityId",
                table: "CandidateUniversities");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "CandidateUniversities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "CandidateUniversities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CandidateUniversities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "CandidateUniversities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateUniversities_Candidates_CandidateId",
                table: "CandidateUniversities",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateUniversities_Universities_UniversityId",
                table: "CandidateUniversities",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
