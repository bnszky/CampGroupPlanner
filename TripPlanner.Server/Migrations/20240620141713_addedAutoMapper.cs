using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TripPlanner.Server.Migrations
{
    /// <inheritdoc />
    public partial class addedAutoMapper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "AuthorUsername",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Reviews",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Reviews",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Regions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Regions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Attractions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "Attractions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "Attractions");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorUsername",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Regions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Regions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cities",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Attractions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Country", "Description", "Name" },
                values: new object[] { 1, "Spain", "Boasting a reputation as one of the most attractive cities in Europe, Barcelona celebrates its role as the capital of Catalonia. The city’s cosmopolitan and international vibe makes it a favorite city for many people around the world. The city is especially known for its architecture and art—travelers flock from around the world to see the iconic Sagrada Família church and other modernist landmarks designed by Gaudí. These Barcelona travel tips just scrape the surface of what can be found in the vibrant city!", "Catalonia" });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CreatedAt", "Description", "EditedAt", "ImageURL", "IsVisible", "PositioningRate", "RegionId", "RegionName", "SourceLink", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque eu vestibulum turpis. Nullam id ipsum at ipsum blandit porttitor sed ut odio. Sed vulputate justo est. Sed tristique, libero eu luctus pellentesque, sem justo luctus nulla, euismod semper quam dui eget mi. Sed at pretium arcu, at gravida nulla. Ut sagittis lacinia ex ut venenatis. Aenean.", new DateTime(2024, 6, 19, 11, 2, 22, 621, DateTimeKind.Local).AddTicks(8288), "https://rodzinanomadow.pl/wp-content/uploads/2018/06/image-10-1024x683.jpeg", true, 4, 1, "Catalonia", "https://www.niagarafallsstatepark.com/", "Beatiful World!" },
                    { 2, new DateTime(2022, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Boasting a reputation as one of the most attractive cities in Europe, Barcelona celebrates its role as the capital of Catalonia.", new DateTime(2024, 6, 19, 11, 2, 22, 621, DateTimeKind.Local).AddTicks(8356), "https://www.theblondeabroad.com/wp-content/uploads/2022/02/theodor-vasile-LSscVPEyQpI-unsplash.jpg", true, 1, 1, "Catalonia", "https://www.theblondeabroad.com/ultimate-barcelona-travel-guide/", "Barcelona" },
                    { 3, new DateTime(2024, 4, 14, 15, 41, 0, 0, DateTimeKind.Unspecified), "Rome is one of the most iconic and most-traveled cities in Europe, with a long history to match. With a mixture of cultures from around the world", new DateTime(2024, 6, 19, 11, 2, 22, 621, DateTimeKind.Local).AddTicks(8360), "https://www.theblondeabroad.com/wp-content/uploads/2022/02/david-edkins-grlIoctRp1o-unsplash.jpg", true, 3, 1, "Catalonia", "https://www.theblondeabroad.com/ultimate-rome-travel-guide/", "Rome" }
                });

            migrationBuilder.InsertData(
                table: "Attractions",
                columns: new[] { "Id", "Description", "FRS_ID", "ImageURL", "Latitude", "Longitude", "Name", "RegionId" },
                values: new object[,]
                {
                    { 1, "    Basilica de la Sagrada Familia is a church devoted to the Holy Family. One of Antoni Gaudis most famous works, the church is perhaps best known for still being under construction since 1882—with works funded purely by donations. Take a lift to the top of the towers for a panoramic view of the city", null, "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/08/10/a7/d6/basilica-de-la-sagrada.jpg?w=1200&h=-1&s=1", 2.1743664953067419, 41.403778921066113, "Basílica de la Sagrada Familia", 1 },
                    { 2, "    Basilica de la Sagrada Familia is a church devoted to the Holy Family. One of Antoni Gaudis most famous works, the church is perhaps best known for still being under construction since 1882—with works funded purely by donations. Take a lift to the top of the towers for a panoramic view of the city", null, "https://lh5.googleusercontent.com/p/AF1QipNgwQHFyIjmdNz9RYHLND4_2hXzrBmqObHjBIfR=w408-h305-k-no", 2.1527803270732719, 41.414679829569799, "Parc Guell", 1 },
                    { 3, "    Welcome to Barcelona's magical house. A Gaudí masterpiece. A unique immersive experience. International Exhibition of the Year 2022. Children free up to 12 years old.", null, "https://dynamic-media-cdn.tripadvisor.com/media/daodao/photo-o/19/ac/b2/a5/caption.jpg?w=1200&h=-1&s=1", 2.1648710224783669, 41.391878307895141, "Casa Batlló", 1 }
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Country", "Latitude", "Longitude", "Name", "RegionId" },
                values: new object[,]
                {
                    { 1, "Spain", 0.0, 0.0, "Barcelona", 1 },
                    { 2, "Spain", 0.0, 0.0, "Tarragona", 1 },
                    { 3, "Spain", 0.0, 0.0, "Girona", 1 }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "AuthorId", "AuthorUsername", "CreatedAt", "Rate", "RegionId", "RegionName", "Text", "Title" },
                values: new object[,]
                {
                    { 1, "0076352e-d763-45c6-92fa-3731f323f01b", "Michal", new DateTime(2024, 4, 14, 15, 41, 0, 0, DateTimeKind.Unspecified), 4.5, 1, "Catalonia", "Beautiful place! I would like to be there again", "Opinion 1" },
                    { 2, "0076352e-d763-45c6-92fa-3731f323f01b", "Michal", new DateTime(2023, 7, 18, 15, 41, 0, 0, DateTimeKind.Unspecified), 2.0, 1, "Catalonia", "I don't like spanish people, Ughh...", "Opinion 2" }
                });
        }
    }
}
