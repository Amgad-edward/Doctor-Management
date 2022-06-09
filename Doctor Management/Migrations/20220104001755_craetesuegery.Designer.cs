﻿// <auto-generated />
using System;
using Doctor_Management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Doctor_Management.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220104001755_craetesuegery")]
    partial class craetesuegery
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.12");

            modelBuilder.Entity("Doctor_Management.Models.Acccount_Reveal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<int>("IdReveal")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.HasIndex("IdReveal");

                    b.ToTable("account_reveal");
                });

            modelBuilder.Entity("Doctor_Management.Models.Account_Enter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<string>("From")
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40)");

                    b.HasKey("Id");

                    b.ToTable("account_enter");
                });

            modelBuilder.Entity("Doctor_Management.Models.Account_Pay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<string>("ToPay")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("account_pay");
                });

            modelBuilder.Entity("Doctor_Management.Models.BlackList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Idcunstomer")
                        .HasColumnType("int");

                    b.Property<string>("Report")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Idcunstomer");

                    b.ToTable("blacklist");
                });

            modelBuilder.Entity("Doctor_Management.Models.Customer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Blood")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Gender")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("NameCustomer")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("varchar(75)");

                    b.Property<string>("Phones")
                        .HasMaxLength(60)
                        .HasColumnType("varchar(60)");

                    b.Property<DateTime>("dateBirth")
                        .HasColumnType("datetime");

                    b.HasKey("ID");

                    b.ToTable("customer");
                });

            modelBuilder.Entity("Doctor_Management.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("TitleJop")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<DateTime>("datestart")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("employee");
                });

            modelBuilder.Entity("Doctor_Management.Models.Fixed_pay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("FixsedAmmount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("Timespan")
                        .HasColumnType("int");

                    b.Property<string>("itemName")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.ToTable("fixed_pay");
                });

            modelBuilder.Entity("Doctor_Management.Models.Informations", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("IdCustomer")
                        .HasColumnType("int");

                    b.Property<string>("Info")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("IdCustomer");

                    b.ToTable("informations");
                });

            modelBuilder.Entity("Doctor_Management.Models.ItemCheckup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Idcustomer")
                        .HasColumnType("int");

                    b.Property<int>("Idreveal")
                        .HasColumnType("int");

                    b.Property<byte[]>("ImageReport")
                        .HasMaxLength(100000000)
                        .HasColumnType("varbinary(100000000)");

                    b.Property<string>("NameCheck")
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40)");

                    b.Property<double>("Resulte")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("Idcustomer");

                    b.HasIndex("Idreveal");

                    b.ToTable("itemcheckup");
                });

            modelBuilder.Entity("Doctor_Management.Models.Loging", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Admin")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Password")
                        .HasMaxLength(8)
                        .HasColumnType("varchar(8)");

                    b.Property<string>("UserName")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.HasKey("Id");

                    b.ToTable("loging");
                });

            modelBuilder.Entity("Doctor_Management.Models.MedicName", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("NameMedic")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("medicname");
                });

            modelBuilder.Entity("Doctor_Management.Models.Messages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("From")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<bool>("ISRead")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Message")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("messages");
                });

            modelBuilder.Entity("Doctor_Management.Models.NamesCheck", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nameschiled")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TitelName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("namescheck");
                });

            modelBuilder.Entity("Doctor_Management.Models.Owner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Addres")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<byte[]>("Logo")
                        .HasMaxLength(10000000)
                        .HasColumnType("varbinary(10000000)");

                    b.Property<string>("NameOwner")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("varchar(70)");

                    b.Property<string>("Phones")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Titel")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("owner");
                });

            modelBuilder.Entity("Doctor_Management.Models.PlaneReveals", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("All")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateDay")
                        .HasColumnType("datetime");

                    b.Property<bool>("Leave")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("planereveals");
                });

            modelBuilder.Entity("Doctor_Management.Models.Price", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("ThePrice")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("Time")
                        .HasColumnType("int");

                    b.Property<string>("genderprice")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.ToTable("price");
                });

            modelBuilder.Entity("Doctor_Management.Models.ResultNormal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Normal")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.ToTable("resultnormal");
                });

            modelBuilder.Entity("Doctor_Management.Models.Reveal", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DateReservation")
                        .HasColumnType("datetime");

                    b.Property<string>("Diagnosis")
                        .HasMaxLength(80)
                        .HasColumnType("varchar(80)");

                    b.Property<bool>("Done")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Idcustomer")
                        .HasColumnType("int");

                    b.Property<int>("Idprice")
                        .HasColumnType("int");

                    b.Property<bool>("IsRe_Reveal")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<DateTime?>("date_Re_Reveal")
                        .HasColumnType("datetime");

                    b.HasKey("ID");

                    b.HasIndex("Idcustomer");

                    b.HasIndex("Idprice");

                    b.ToTable("reveal");
                });

            modelBuilder.Entity("Doctor_Management.Models.Surgery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime");

                    b.Property<bool>("Done")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("IdCustomer")
                        .HasColumnType("int");

                    b.Property<string>("NameSurgery")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.HasIndex("IdCustomer");

                    b.ToTable("surgery");
                });

            modelBuilder.Entity("Doctor_Management.Models.Therapy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Idmedic")
                        .HasColumnType("int");

                    b.Property<int>("Idreveal")
                        .HasColumnType("int");

                    b.Property<string>("Sub")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("Idmedic");

                    b.HasIndex("Idreveal");

                    b.ToTable("therapy");
                });

            modelBuilder.Entity("Doctor_Management.Models.Acccount_Reveal", b =>
                {
                    b.HasOne("Doctor_Management.Models.Reveal", "reveal")
                        .WithMany()
                        .HasForeignKey("IdReveal")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("reveal");
                });

            modelBuilder.Entity("Doctor_Management.Models.BlackList", b =>
                {
                    b.HasOne("Doctor_Management.Models.Customer", "customer")
                        .WithMany()
                        .HasForeignKey("Idcunstomer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("customer");
                });

            modelBuilder.Entity("Doctor_Management.Models.Informations", b =>
                {
                    b.HasOne("Doctor_Management.Models.Customer", "customer")
                        .WithMany("Informations")
                        .HasForeignKey("IdCustomer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("customer");
                });

            modelBuilder.Entity("Doctor_Management.Models.ItemCheckup", b =>
                {
                    b.HasOne("Doctor_Management.Models.Customer", "customer")
                        .WithMany("ItemCheckups")
                        .HasForeignKey("Idcustomer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Doctor_Management.Models.Reveal", "reveal")
                        .WithMany("ItemCheckups")
                        .HasForeignKey("Idreveal")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("customer");

                    b.Navigation("reveal");
                });

            modelBuilder.Entity("Doctor_Management.Models.Reveal", b =>
                {
                    b.HasOne("Doctor_Management.Models.Customer", "customer")
                        .WithMany("Reveals")
                        .HasForeignKey("Idcustomer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Doctor_Management.Models.Price", "price")
                        .WithMany()
                        .HasForeignKey("Idprice")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("customer");

                    b.Navigation("price");
                });

            modelBuilder.Entity("Doctor_Management.Models.Surgery", b =>
                {
                    b.HasOne("Doctor_Management.Models.Customer", "customer")
                        .WithMany()
                        .HasForeignKey("IdCustomer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("customer");
                });

            modelBuilder.Entity("Doctor_Management.Models.Therapy", b =>
                {
                    b.HasOne("Doctor_Management.Models.MedicName", "medicName")
                        .WithMany()
                        .HasForeignKey("Idmedic")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Doctor_Management.Models.Reveal", "reveal")
                        .WithMany("Therapies")
                        .HasForeignKey("Idreveal")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("medicName");

                    b.Navigation("reveal");
                });

            modelBuilder.Entity("Doctor_Management.Models.Customer", b =>
                {
                    b.Navigation("Informations");

                    b.Navigation("ItemCheckups");

                    b.Navigation("Reveals");
                });

            modelBuilder.Entity("Doctor_Management.Models.Reveal", b =>
                {
                    b.Navigation("ItemCheckups");

                    b.Navigation("Therapies");
                });
#pragma warning restore 612, 618
        }
    }
}
