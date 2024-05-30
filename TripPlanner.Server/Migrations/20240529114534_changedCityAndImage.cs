using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripPlanner.Server.Migrations
{
    /// <inheritdoc />
    public partial class changedCityAndImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ImageURLs");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Cities",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Cities",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Country", "Latitude", "Longitude" },
                values: new object[] { null, 0.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Country", "Latitude", "Longitude" },
                values: new object[] { null, 0.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Country", "Latitude", "Longitude" },
                values: new object[] { null, 0.0, 0.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Cities");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ImageURLs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ImageURLs",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "image1");

            migrationBuilder.UpdateData(
                table: "ImageURLs",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "image2");
        }
    }
}
