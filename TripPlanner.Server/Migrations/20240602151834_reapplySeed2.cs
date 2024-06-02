using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TripPlanner.Server.Migrations
{
    /// <inheritdoc />
    public partial class reapplySeed2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CreatedAt", "Description", "EditedAt", "ImageURL", "IsVisible", "PositioningRate", "RegionId", "RegionName", "SourceLink", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque eu vestibulum turpis. Nullam id ipsum at ipsum blandit porttitor sed ut odio. Sed vulputate justo est. Sed tristique, libero eu luctus pellentesque, sem justo luctus nulla, euismod semper quam dui eget mi. Sed at pretium arcu, at gravida nulla. Ut sagittis lacinia ex ut venenatis. Aenean.", new DateTime(2024, 6, 2, 17, 18, 34, 8, DateTimeKind.Local).AddTicks(560), "https://rodzinanomadow.pl/wp-content/uploads/2018/06/image-10-1024x683.jpeg", true, 4, 1, "Catalonia", "https://www.niagarafallsstatepark.com/", "Beatiful World!" },
                    { 2, new DateTime(2022, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Boasting a reputation as one of the most attractive cities in Europe, Barcelona celebrates its role as the capital of Catalonia.", new DateTime(2024, 6, 2, 17, 18, 34, 8, DateTimeKind.Local).AddTicks(621), "https://www.theblondeabroad.com/wp-content/uploads/2022/02/theodor-vasile-LSscVPEyQpI-unsplash.jpg", true, 1, 1, "Catalonia", "https://www.theblondeabroad.com/ultimate-barcelona-travel-guide/", "Barcelona" },
                    { 3, new DateTime(2024, 4, 14, 15, 41, 0, 0, DateTimeKind.Unspecified), "Rome is one of the most iconic and most-traveled cities in Europe, with a long history to match. With a mixture of cultures from around the world", new DateTime(2024, 6, 2, 17, 18, 34, 8, DateTimeKind.Local).AddTicks(625), "https://www.theblondeabroad.com/wp-content/uploads/2022/02/david-edkins-grlIoctRp1o-unsplash.jpg", true, 3, 1, "Catalonia", "https://www.theblondeabroad.com/ultimate-rome-travel-guide/", "Rome" }
                });

            migrationBuilder.InsertData(
                table: "Attractions",
                columns: new[] { "Id", "Description", "ImageURL", "Latitude", "Longitude", "Name", "RegionId" },
                values: new object[,]
                {
                    { 1, "    Basilica de la Sagrada Familia is a church devoted to the Holy Family. One of Antoni Gaudis most famous works, the church is perhaps best known for still being under construction since 1882—with works funded purely by donations. Take a lift to the top of the towers for a panoramic view of the city", "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/08/10/a7/d6/basilica-de-la-sagrada.jpg?w=1200&h=-1&s=1", 2.1743664953067419, 41.403778921066113, "Basílica de la Sagrada Familia", 1 },
                    { 2, "    Basilica de la Sagrada Familia is a church devoted to the Holy Family. One of Antoni Gaudis most famous works, the church is perhaps best known for still being under construction since 1882—with works funded purely by donations. Take a lift to the top of the towers for a panoramic view of the city", "https://lh5.googleusercontent.com/p/AF1QipNgwQHFyIjmdNz9RYHLND4_2hXzrBmqObHjBIfR=w408-h305-k-no", 2.1527803270732719, 41.414679829569799, "Parc Guell", 1 },
                    { 3, "    Welcome to Barcelona's magical house. A Gaudí masterpiece. A unique immersive experience. International Exhibition of the Year 2022. Children free up to 12 years old.", "https://dynamic-media-cdn.tripadvisor.com/media/daodao/photo-o/19/ac/b2/a5/caption.jpg?w=1200&h=-1&s=1", 2.1648710224783669, 41.391878307895141, "Casa Batlló", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
