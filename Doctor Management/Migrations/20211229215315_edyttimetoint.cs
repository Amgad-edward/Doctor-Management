using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Doctor_Management.Migrations
{
    public partial class edyttimetoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Time",
                table: "price",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "price",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
