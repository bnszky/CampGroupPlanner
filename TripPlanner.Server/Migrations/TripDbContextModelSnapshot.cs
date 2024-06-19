﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TripPlanner.Server.Data;

#nullable disable

namespace TripPlanner.Server.Migrations
{
    [DbContext(typeof(TripDbContext))]
    partial class TripDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.3.24172.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("TripPlanner.Server.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("EditedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<int>("PositioningRate")
                        .HasColumnType("int");

                    b.Property<int?>("RegionId")
                        .HasColumnType("int");

                    b.Property<string>("RegionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SourceLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Articles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2020, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque eu vestibulum turpis. Nullam id ipsum at ipsum blandit porttitor sed ut odio. Sed vulputate justo est. Sed tristique, libero eu luctus pellentesque, sem justo luctus nulla, euismod semper quam dui eget mi. Sed at pretium arcu, at gravida nulla. Ut sagittis lacinia ex ut venenatis. Aenean.",
                            EditedAt = new DateTime(2024, 6, 19, 11, 2, 22, 621, DateTimeKind.Local).AddTicks(8288),
                            ImageURL = "https://rodzinanomadow.pl/wp-content/uploads/2018/06/image-10-1024x683.jpeg",
                            IsVisible = true,
                            PositioningRate = 4,
                            RegionId = 1,
                            RegionName = "Catalonia",
                            SourceLink = "https://www.niagarafallsstatepark.com/",
                            Title = "Beatiful World!"
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2022, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Boasting a reputation as one of the most attractive cities in Europe, Barcelona celebrates its role as the capital of Catalonia.",
                            EditedAt = new DateTime(2024, 6, 19, 11, 2, 22, 621, DateTimeKind.Local).AddTicks(8356),
                            ImageURL = "https://www.theblondeabroad.com/wp-content/uploads/2022/02/theodor-vasile-LSscVPEyQpI-unsplash.jpg",
                            IsVisible = true,
                            PositioningRate = 1,
                            RegionId = 1,
                            RegionName = "Catalonia",
                            SourceLink = "https://www.theblondeabroad.com/ultimate-barcelona-travel-guide/",
                            Title = "Barcelona"
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2024, 4, 14, 15, 41, 0, 0, DateTimeKind.Unspecified),
                            Description = "Rome is one of the most iconic and most-traveled cities in Europe, with a long history to match. With a mixture of cultures from around the world",
                            EditedAt = new DateTime(2024, 6, 19, 11, 2, 22, 621, DateTimeKind.Local).AddTicks(8360),
                            ImageURL = "https://www.theblondeabroad.com/wp-content/uploads/2022/02/david-edkins-grlIoctRp1o-unsplash.jpg",
                            IsVisible = true,
                            PositioningRate = 3,
                            RegionId = 1,
                            RegionName = "Catalonia",
                            SourceLink = "https://www.theblondeabroad.com/ultimate-rome-travel-guide/",
                            Title = "Rome"
                        });
                });

            modelBuilder.Entity("TripPlanner.Server.Models.Attraction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FRS_ID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("RegionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Attractions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "    Basilica de la Sagrada Familia is a church devoted to the Holy Family. One of Antoni Gaudis most famous works, the church is perhaps best known for still being under construction since 1882—with works funded purely by donations. Take a lift to the top of the towers for a panoramic view of the city",
                            ImageURL = "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/08/10/a7/d6/basilica-de-la-sagrada.jpg?w=1200&h=-1&s=1",
                            Latitude = 2.1743664953067419,
                            Longitude = 41.403778921066113,
                            Name = "Basílica de la Sagrada Familia",
                            RegionId = 1
                        },
                        new
                        {
                            Id = 2,
                            Description = "    Basilica de la Sagrada Familia is a church devoted to the Holy Family. One of Antoni Gaudis most famous works, the church is perhaps best known for still being under construction since 1882—with works funded purely by donations. Take a lift to the top of the towers for a panoramic view of the city",
                            ImageURL = "https://lh5.googleusercontent.com/p/AF1QipNgwQHFyIjmdNz9RYHLND4_2hXzrBmqObHjBIfR=w408-h305-k-no",
                            Latitude = 2.1527803270732719,
                            Longitude = 41.414679829569799,
                            Name = "Parc Guell",
                            RegionId = 1
                        },
                        new
                        {
                            Id = 3,
                            Description = "    Welcome to Barcelona's magical house. A Gaudí masterpiece. A unique immersive experience. International Exhibition of the Year 2022. Children free up to 12 years old.",
                            ImageURL = "https://dynamic-media-cdn.tripadvisor.com/media/daodao/photo-o/19/ac/b2/a5/caption.jpg?w=1200&h=-1&s=1",
                            Latitude = 2.1648710224783669,
                            Longitude = 41.391878307895141,
                            Name = "Casa Batlló",
                            RegionId = 1
                        });
                });

            modelBuilder.Entity("TripPlanner.Server.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("RegionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("RegionId");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Country = "Spain",
                            Latitude = 0.0,
                            Longitude = 0.0,
                            Name = "Barcelona",
                            RegionId = 1
                        },
                        new
                        {
                            Id = 2,
                            Country = "Spain",
                            Latitude = 0.0,
                            Longitude = 0.0,
                            Name = "Tarragona",
                            RegionId = 1
                        },
                        new
                        {
                            Id = 3,
                            Country = "Spain",
                            Latitude = 0.0,
                            Longitude = 0.0,
                            Name = "Girona",
                            RegionId = 1
                        });
                });

            modelBuilder.Entity("TripPlanner.Server.Models.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Regions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Country = "Spain",
                            Description = "Boasting a reputation as one of the most attractive cities in Europe, Barcelona celebrates its role as the capital of Catalonia. The city’s cosmopolitan and international vibe makes it a favorite city for many people around the world. The city is especially known for its architecture and art—travelers flock from around the world to see the iconic Sagrada Família church and other modernist landmarks designed by Gaudí. These Barcelona travel tips just scrape the surface of what can be found in the vibrant city!",
                            Name = "Catalonia"
                        });
                });

            modelBuilder.Entity("TripPlanner.Server.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AuthorUsername")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<double>("Rate")
                        .HasColumnType("float");

                    b.Property<int>("RegionId")
                        .HasColumnType("int");

                    b.Property<string>("RegionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("RegionId");

                    b.ToTable("Reviews");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorId = "0076352e-d763-45c6-92fa-3731f323f01b",
                            AuthorUsername = "Michal",
                            CreatedAt = new DateTime(2024, 4, 14, 15, 41, 0, 0, DateTimeKind.Unspecified),
                            Rate = 4.5,
                            RegionId = 1,
                            RegionName = "Catalonia",
                            Text = "Beautiful place! I would like to be there again",
                            Title = "Opinion 1"
                        },
                        new
                        {
                            Id = 2,
                            AuthorId = "0076352e-d763-45c6-92fa-3731f323f01b",
                            AuthorUsername = "Michal",
                            CreatedAt = new DateTime(2023, 7, 18, 15, 41, 0, 0, DateTimeKind.Unspecified),
                            Rate = 2.0,
                            RegionId = 1,
                            RegionName = "Catalonia",
                            Text = "I don't like spanish people, Ughh...",
                            Title = "Opinion 2"
                        });
                });

            modelBuilder.Entity("TripPlanner.Server.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.HasOne("TripPlanner.Server.Models.User", null)
                        .WithMany("Roles")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TripPlanner.Server.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TripPlanner.Server.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TripPlanner.Server.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TripPlanner.Server.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TripPlanner.Server.Models.Article", b =>
                {
                    b.HasOne("TripPlanner.Server.Models.Region", "Region")
                        .WithMany("Articles")
                        .HasForeignKey("RegionId");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("TripPlanner.Server.Models.Attraction", b =>
                {
                    b.HasOne("TripPlanner.Server.Models.Region", "Region")
                        .WithMany("Attractions")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Region");
                });

            modelBuilder.Entity("TripPlanner.Server.Models.City", b =>
                {
                    b.HasOne("TripPlanner.Server.Models.Region", "Region")
                        .WithMany("Cities")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Region");
                });

            modelBuilder.Entity("TripPlanner.Server.Models.Review", b =>
                {
                    b.HasOne("TripPlanner.Server.Models.User", "Author")
                        .WithMany("Reviews")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TripPlanner.Server.Models.Region", "Region")
                        .WithMany("Reviews")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("TripPlanner.Server.Models.Region", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("Attractions");

                    b.Navigation("Cities");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("TripPlanner.Server.Models.User", b =>
                {
                    b.Navigation("Reviews");

                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
