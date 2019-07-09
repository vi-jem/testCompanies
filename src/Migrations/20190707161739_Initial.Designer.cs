﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using myrestful.Infrastructure;

namespace myrestful.Migrations
{
    [DbContext(typeof(DBContextEmployees))]
    [Migration("20190707161739_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("myrestful.Models.Company", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EstablishmentYear");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("myrestful.Models.Employee", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CompanyID");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("FirstName");

                    b.Property<int>("JobTitle");

                    b.Property<string>("LastName");

                    b.HasKey("ID");

                    b.HasIndex("CompanyID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("myrestful.Models.Employee", b =>
                {
                    b.HasOne("myrestful.Models.Company", "Company")
                        .WithMany("Employees")
                        .HasForeignKey("CompanyID");
                });
#pragma warning restore 612, 618
        }
    }
}