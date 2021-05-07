using Microsoft.EntityFrameworkCore.Migrations;

namespace KeywordsApp.Migrations
{
    public partial class Addkeywordparsingresults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdWordsCount",
                table: "Keywords",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "HtmlCode",
                table: "Keywords",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LinkCount",
                table: "Keywords",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequestDuration",
                table: "Keywords",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalThouthandResultsCount",
                table: "Keywords",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdWordsCount",
                table: "Keywords");

            migrationBuilder.DropColumn(
                name: "HtmlCode",
                table: "Keywords");

            migrationBuilder.DropColumn(
                name: "LinkCount",
                table: "Keywords");

            migrationBuilder.DropColumn(
                name: "RequestDuration",
                table: "Keywords");

            migrationBuilder.DropColumn(
                name: "TotalThouthandResultsCount",
                table: "Keywords");
        }
    }
}
