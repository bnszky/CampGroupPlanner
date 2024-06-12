using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripPlanner.Server.Migrations
{
    /// <inheritdoc />
    public partial class addedFRS_ID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FRS_ID",
                table: "Attractions",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 1,
                column: "FRS_ID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 2,
                column: "FRS_ID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Attractions",
                keyColumn: "Id",
                keyValue: 3,
                column: "FRS_ID",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FRS_ID",
                table: "Attractions");

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
        }
    }
}
