using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TripPlanner.Server.Migrations
{
    /// <inheritdoc />
    public partial class removedImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageURLs");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "EditedAt",
                value: new DateTime(2024, 6, 19, 11, 2, 22, 621, DateTimeKind.Local).AddTicks(8288));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                column: "EditedAt",
                value: new DateTime(2024, 6, 19, 11, 2, 22, 621, DateTimeKind.Local).AddTicks(8356));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                column: "EditedAt",
                value: new DateTime(2024, 6, 19, 11, 2, 22, 621, DateTimeKind.Local).AddTicks(8360));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImageURLs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "EditedAt",
                value: new DateTime(2024, 6, 12, 23, 47, 52, 895, DateTimeKind.Local).AddTicks(8575));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                column: "EditedAt",
                value: new DateTime(2024, 6, 12, 23, 47, 52, 895, DateTimeKind.Local).AddTicks(8633));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                column: "EditedAt",
                value: new DateTime(2024, 6, 12, 23, 47, 52, 895, DateTimeKind.Local).AddTicks(8641));

            migrationBuilder.InsertData(
                table: "ImageURLs",
                columns: new[] { "Id", "Link", "RegionId" },
                values: new object[,]
                {
                    { 1, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRnwf8dsSKIsCsVbwXlpQEuvEP6q70MdNVjdQ&s", 1 },
                    { 2, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRaRfTP8AW7Od72m4IRi4LPRt9xNqPYfYlPrg&s", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageURLs_RegionId",
                table: "ImageURLs",
                column: "RegionId");
        }
    }
}
