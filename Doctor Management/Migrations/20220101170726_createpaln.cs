using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace Doctor_Management.Migrations
{
    public partial class createpaln : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase(
                oldCollation: "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "planereveals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateDay = table.Column<DateTime>(type: "datetime", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    All = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Leave = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_planereveals", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "planereveals");

            migrationBuilder.AlterDatabase(
                collation: "utf8_general_ci");
        }
    }
}
