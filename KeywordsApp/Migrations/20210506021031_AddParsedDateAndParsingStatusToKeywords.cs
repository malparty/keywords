using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KeywordsApp.Migrations
{
    public partial class AddParsedDateAndParsingStatusToKeywords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ParsedDate",
                table: "Keywords",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParsingStatus",
                table: "Keywords",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParsedDate",
                table: "Keywords");

            migrationBuilder.DropColumn(
                name: "ParsingStatus",
                table: "Keywords");
        }
    }
}
