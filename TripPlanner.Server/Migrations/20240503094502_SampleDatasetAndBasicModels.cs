using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TripPlanner.Server.Migrations
{
    /// <inheritdoc />
    public partial class SampleDatasetAndBasicModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Attractions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nick = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SourceLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageURLs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageURLs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageURLs_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegionId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 2,
                column: "RegionId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 3,
                column: "RegionId",
                value: 1);

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Country", "Description", "Name" },
                values: new object[] { 1, "Spain", "Boasting a reputation as one of the most attractive cities in Europe, Barcelona celebrates its role as the capital of Catalonia. The city’s cosmopolitan and international vibe makes it a favorite city for many people around the world. The city is especially known for its architecture and art—travelers flock from around the world to see the iconic Sagrada Família church and other modernist landmarks designed by Gaudí. These Barcelona travel tips just scrape the surface of what can be found in the vibrant city!", "Catalonia" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Nick", "ProfileImageUrl" },
                values: new object[,]
                {
                    { 1, "michali@example.com", "Michali", "LastName1", "michali", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSGDohX4qAelLzi3t8vCfqccDFxifY-huxkmRrgnSRoig&s" },
                    { 2, "gombalo@example.com", "Gombalo", "LastName2", "gombalo", "https://static.vecteezy.com/system/resources/thumbnails/002/002/403/small/man-with-beard-avatar-character-isolated-icon-free-vector.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CreatedAt", "Description", "ImageURL", "RegionId", "SourceLink", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque eu vestibulum turpis. Maecenas placerat ac metus eget maximus. Suspendisse fermentum, eros a luctus laoreet, nunc nisl mattis turpis, quis tincidunt erat urna non lorem. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Cras ornare lectus ac neque rhoncus, vitae consectetur arcu pretium. Nullam id ipsum at ipsum blandit porttitor sed ut odio. Sed vulputate justo est. Sed tristique, libero eu luctus pellentesque, sem justo luctus nulla, euismod semper quam dui eget mi. Sed at pretium arcu, at gravida nulla. Ut sagittis lacinia ex ut venenatis. Aenean.", "https://rodzinanomadow.pl/wp-content/uploads/2018/06/image-10-1024x683.jpeg", 1, "https://www.niagarafallsstatepark.com/", "Beatiful World!" },
                    { 2, new DateTime(2022, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Boasting a reputation as one of the most attractive cities in Europe, Barcelona celebrates its role as the capital of Catalonia. The city’s cosmopolitan and international vibe makes it a favorite city for many people around the world. The city is especially known for its architecture and art—travelers flock from around the world to see the iconic Sagrada Família church and other modernist landmarks designed by Gaudí. These Barcelona travel tips just scrape the surface of what can be found in the vibrant city!", "https://www.theblondeabroad.com/wp-content/uploads/2022/02/theodor-vasile-LSscVPEyQpI-unsplash.jpg", 1, "https://www.theblondeabroad.com/ultimate-barcelona-travel-guide/", "Barcelona" },
                    { 3, new DateTime(2024, 4, 14, 15, 41, 0, 0, DateTimeKind.Unspecified), "Rome is one of the most iconic and most-traveled cities in Europe, with a long history to match. With a mixture of cultures from around the world, Rome has it all. Wander the cobblestone street with gelato in hand, spend some time people watching from the Spanish Steps, spend hours in the museums, and take in all of the stunning architecture the city has to offer. Rome is also a foodie's heaven, from the world-class cuisine to casual trattorias and takeaway pizzas. Here's my ultimate Rome travel guide!", "https://www.theblondeabroad.com/wp-content/uploads/2022/02/david-edkins-grlIoctRp1o-unsplash.jpg", 1, "https://www.theblondeabroad.com/ultimate-rome-travel-guide/", "Rome" }
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name", "RegionId" },
                values: new object[,]
                {
                    { 1, "Barcelona", 1 },
                    { 2, "Tarragona", 1 },
                    { 3, "Girona", 1 }
                });

            migrationBuilder.InsertData(
                table: "ImageURLs",
                columns: new[] { "Id", "Link", "Name", "RegionId" },
                values: new object[,]
                {
                    { 1, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRnwf8dsSKIsCsVbwXlpQEuvEP6q70MdNVjdQ&s", "image1", 1 },
                    { 2, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRaRfTP8AW7Od72m4IRi4LPRt9xNqPYfYlPrg&s", "image2", 1 }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "AuthorId", "CreatedAt", "Rate", "RegionId", "Text", "Title" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 4, 14, 15, 41, 0, 0, DateTimeKind.Unspecified), 4.5, 1, "Beautiful place! I would like to be there again", "Opinion 1" },
                    { 2, 2, new DateTime(2023, 7, 18, 15, 41, 0, 0, DateTimeKind.Unspecified), 2.0, 1, "I don't like spanish people, Ughh...", "Opinion 2" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attractions_RegionId",
                table: "Attractions",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_RegionId",
                table: "Articles",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_RegionId",
                table: "Cities",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageURLs_RegionId",
                table: "ImageURLs",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AuthorId",
                table: "Reviews",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RegionId",
                table: "Reviews",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attractions_Regions_RegionId",
                table: "Attractions",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_Regions_RegionId",
                table: "Attractions");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "ImageURLs");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Attractions_RegionId",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Attractions");
        }
    }
}
