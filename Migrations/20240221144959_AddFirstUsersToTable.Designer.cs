﻿// <auto-generated />
using CampGroupPlanner.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CampGroupPlanner.Migrations
{
    [DbContext(typeof(AppDbController))]
    [Migration("20240221144959_AddFirstUsersToTable")]
    partial class AddFirstUsersToTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.1.23111.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CampGroupPlanner.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nick")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "michal@onet.eu",
                            FirstName = "Michal",
                            LastName = "Kowalski",
                            Nick = ""
                        },
                        new
                        {
                            Id = 2,
                            Email = "adam@gmail.com",
                            FirstName = "Adam",
                            LastName = "Dobrek",
                            Nick = ""
                        },
                        new
                        {
                            Id = 3,
                            Email = "karolina@gmail.com",
                            FirstName = "Caroline",
                            LastName = "Koralińska",
                            Nick = ""
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
