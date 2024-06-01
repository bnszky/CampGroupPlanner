using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripPlanner.Server.Migrations
{
    /// <inheritdoc />
    public partial class changedArticles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EditedAt", "IsVisible", "PositioningRate", "RegionName" },
                values: new object[] { new DateTime(2024, 6, 1, 15, 1, 25, 803, DateTimeKind.Local).AddTicks(4006), true, 4, "Catalonia" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EditedAt", "IsVisible", "PositioningRate", "RegionName" },
                values: new object[] { new DateTime(2024, 6, 1, 15, 1, 25, 803, DateTimeKind.Local).AddTicks(4064), true, 1, "Catalonia" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EditedAt", "IsVisible", "PositioningRate", "RegionName" },
                values: new object[] { new DateTime(2024, 6, 1, 15, 1, 25, 803, DateTimeKind.Local).AddTicks(4067), true, 3, "Catalonia" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EditedAt", "IsVisible", "PositioningRate", "RegionName" },
                values: new object[] { new DateTime(2024, 6, 1, 14, 55, 18, 472, DateTimeKind.Local).AddTicks(8379), false, 0, null });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EditedAt", "IsVisible", "PositioningRate", "RegionName" },
                values: new object[] { new DateTime(2024, 6, 1, 14, 55, 18, 472, DateTimeKind.Local).AddTicks(8444), false, 0, null });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EditedAt", "IsVisible", "PositioningRate", "RegionName" },
                values: new object[] { new DateTime(2024, 6, 1, 14, 55, 18, 472, DateTimeKind.Local).AddTicks(8447), false, 0, null });
        }
    }
}
