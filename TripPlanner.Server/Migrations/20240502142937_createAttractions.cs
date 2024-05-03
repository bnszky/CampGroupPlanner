using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TripPlanner.Server.Migrations
{
    /// <inheritdoc />
    public partial class createAttractions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attractions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attractions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Attractions",
                columns: new[] { "Id", "Description", "ImageURL", "Latitude", "Longitude", "Name" },
                values: new object[,]
                {
                    { 1, "    Basilica de la Sagrada Familia is a church devoted to the Holy Family. One of Antoni Gaudis most famous works, the church is perhaps best known for still being under construction since 1882—with works funded purely by donations. Take a lift to the top of the towers for a panoramic view of the city", "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/08/10/a7/d6/basilica-de-la-sagrada.jpg?w=1200&h=-1&s=1", 2.1743664953067419, 41.403778921066113, "Basílica de la Sagrada Familia" },
                    { 2, "    Basilica de la Sagrada Familia is a church devoted to the Holy Family. One of Antoni Gaudis most famous works, the church is perhaps best known for still being under construction since 1882—with works funded purely by donations. Take a lift to the top of the towers for a panoramic view of the city", "https://lh5.googleusercontent.com/p/AF1QipNgwQHFyIjmdNz9RYHLND4_2hXzrBmqObHjBIfR=w408-h305-k-no", 2.1527803270732719, 41.414679829569799, "Parc Guell" },
                    { 3, "    Welcome to Barcelona's magical house. A Gaudí masterpiece. A unique immersive experience. International Exhibition of the Year 2022. Children free up to 12 years old.", "https://dynamic-media-cdn.tripadvisor.com/media/daodao/photo-o/19/ac/b2/a5/caption.jpg?w=1200&h=-1&s=1", 2.1648710224783669, 41.391878307895141, "Casa Batlló" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attractions");
        }
    }
}
