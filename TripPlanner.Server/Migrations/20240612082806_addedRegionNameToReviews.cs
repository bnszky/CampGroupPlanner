using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripPlanner.Server.Migrations
{
    /// <inheritdoc />
    public partial class addedRegionNameToReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "EditedAt",
                value: new DateTime(2024, 6, 12, 10, 28, 5, 859, DateTimeKind.Local).AddTicks(9392));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                column: "EditedAt",
                value: new DateTime(2024, 6, 12, 10, 28, 5, 859, DateTimeKind.Local).AddTicks(9460));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                column: "EditedAt",
                value: new DateTime(2024, 6, 12, 10, 28, 5, 859, DateTimeKind.Local).AddTicks(9464));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegionName",
                value: "Catalonia");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "RegionName",
                value: "Catalonia");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "Reviews");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "EditedAt",
                value: new DateTime(2024, 6, 9, 23, 26, 2, 473, DateTimeKind.Local).AddTicks(3679));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                column: "EditedAt",
                value: new DateTime(2024, 6, 9, 23, 26, 2, 473, DateTimeKind.Local).AddTicks(3746));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                column: "EditedAt",
                value: new DateTime(2024, 6, 9, 23, 26, 2, 473, DateTimeKind.Local).AddTicks(3749));
        }
    }
}
