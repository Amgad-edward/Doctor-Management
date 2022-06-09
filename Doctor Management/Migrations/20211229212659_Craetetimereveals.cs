using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Doctor_Management.Migrations
{
    public partial class Craetetimereveals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Time",
                table: "price",
                type: "time",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "price");
        }
    }
}
