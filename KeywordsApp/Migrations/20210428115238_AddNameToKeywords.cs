﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace KeywordsApp.Migrations
{
    public partial class AddNameToKeywords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Keywords",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Keywords");
        }
    }
}