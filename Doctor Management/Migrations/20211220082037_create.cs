using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace Doctor_Management.Migrations
{
    public partial class create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account_enter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    From = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_enter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "account_pay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ToPay = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_pay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    NameCustomer = table.Column<string>(type: "varchar(75)", maxLength: 75, nullable: false),
                    Phones = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true),
                    Gender = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    Blood = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    dateBirth = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    datestart = table.Column<DateTime>(type: "datetime", nullable: false),
                    TitleJop = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "fixed_pay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    itemName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    FixsedAmmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Timespan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fixed_pay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "loging",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    Password = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true),
                    Admin = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loging", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "medicname",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    NameMedic = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medicname", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "namescheck",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TitelName = table.Column<string>(type: "text", nullable: false),
                    Nameschiled = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_namescheck", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "owner",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    NameOwner = table.Column<string>(type: "varchar(70)", maxLength: 70, nullable: false),
                    Titel = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Addres = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Phones = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Logo = table.Column<byte[]>(type: "varbinary(10000000)", maxLength: 10000000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_owner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "price",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    genderprice = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    ThePrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_price", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "resultnormal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Normal = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resultnormal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "blacklist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Idcunstomer = table.Column<int>(type: "int", nullable: false),
                    Report = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blacklist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_blacklist_customer_Idcunstomer",
                        column: x => x.Idcunstomer,
                        principalTable: "customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "informations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    IdCustomer = table.Column<int>(type: "int", nullable: false),
                    Info = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_informations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_informations_customer_IdCustomer",
                        column: x => x.IdCustomer,
                        principalTable: "customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reveal",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateReservation = table.Column<DateTime>(type: "datetime", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Idcustomer = table.Column<int>(type: "int", nullable: false),
                    Idprice = table.Column<int>(type: "int", nullable: false),
                    Done = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Diagnosis = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true),
                    IsRe_Reveal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    date_Re_Reveal = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reveal", x => x.ID);
                    table.ForeignKey(
                        name: "FK_reveal_customer_Idcustomer",
                        column: x => x.Idcustomer,
                        principalTable: "customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reveal_price_Idprice",
                        column: x => x.Idprice,
                        principalTable: "price",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_reveal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    IdReveal = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_reveal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_account_reveal_reveal_IdReveal",
                        column: x => x.IdReveal,
                        principalTable: "reveal",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "itemcheckup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Idreveal = table.Column<int>(type: "int", nullable: false),
                    Idcustomer = table.Column<int>(type: "int", nullable: false),
                    NameCheck = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    Resulte = table.Column<double>(type: "double", nullable: false),
                    ImageReport = table.Column<byte[]>(type: "varbinary(100000000)", maxLength: 100000000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itemcheckup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_itemcheckup_customer_Idcustomer",
                        column: x => x.Idcustomer,
                        principalTable: "customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_itemcheckup_reveal_Idreveal",
                        column: x => x.Idreveal,
                        principalTable: "reveal",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "therapy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Idreveal = table.Column<int>(type: "int", nullable: false),
                    Idmedic = table.Column<int>(type: "int", nullable: false),
                    Sub = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_therapy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_therapy_medicname_Idmedic",
                        column: x => x.Idmedic,
                        principalTable: "medicname",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_therapy_reveal_Idreveal",
                        column: x => x.Idreveal,
                        principalTable: "reveal",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_reveal_IdReveal",
                table: "account_reveal",
                column: "IdReveal");

            migrationBuilder.CreateIndex(
                name: "IX_blacklist_Idcunstomer",
                table: "blacklist",
                column: "Idcunstomer");

            migrationBuilder.CreateIndex(
                name: "IX_informations_IdCustomer",
                table: "informations",
                column: "IdCustomer");

            migrationBuilder.CreateIndex(
                name: "IX_itemcheckup_Idcustomer",
                table: "itemcheckup",
                column: "Idcustomer");

            migrationBuilder.CreateIndex(
                name: "IX_itemcheckup_Idreveal",
                table: "itemcheckup",
                column: "Idreveal");

            migrationBuilder.CreateIndex(
                name: "IX_reveal_Idcustomer",
                table: "reveal",
                column: "Idcustomer");

            migrationBuilder.CreateIndex(
                name: "IX_reveal_Idprice",
                table: "reveal",
                column: "Idprice");

            migrationBuilder.CreateIndex(
                name: "IX_therapy_Idmedic",
                table: "therapy",
                column: "Idmedic");

            migrationBuilder.CreateIndex(
                name: "IX_therapy_Idreveal",
                table: "therapy",
                column: "Idreveal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_enter");

            migrationBuilder.DropTable(
                name: "account_pay");

            migrationBuilder.DropTable(
                name: "account_reveal");

            migrationBuilder.DropTable(
                name: "blacklist");

            migrationBuilder.DropTable(
                name: "employee");

            migrationBuilder.DropTable(
                name: "fixed_pay");

            migrationBuilder.DropTable(
                name: "informations");

            migrationBuilder.DropTable(
                name: "itemcheckup");

            migrationBuilder.DropTable(
                name: "loging");

            migrationBuilder.DropTable(
                name: "namescheck");

            migrationBuilder.DropTable(
                name: "owner");

            migrationBuilder.DropTable(
                name: "resultnormal");

            migrationBuilder.DropTable(
                name: "therapy");

            migrationBuilder.DropTable(
                name: "medicname");

            migrationBuilder.DropTable(
                name: "reveal");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "price");
        }
    }
}
