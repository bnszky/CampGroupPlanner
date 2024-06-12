using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripPlanner.Server.Migrations
{
    /// <inheritdoc />
    public partial class addedAuthorUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorUsername",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "AuthorUsername",
                value: "Michal");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "AuthorUsername",
                value: "Michal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorUsername",
                table: "Reviews");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "EditedAt",
                value: new DateTime(2024, 6, 5, 10, 38, 55, 733, DateTimeKind.Local).AddTicks(5736));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                column: "EditedAt",
                value: new DateTime(2024, 6, 5, 10, 38, 55, 733, DateTimeKind.Local).AddTicks(5806));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                column: "EditedAt",
                value: new DateTime(2024, 6, 5, 10, 38, 55, 733, DateTimeKind.Local).AddTicks(5809));
        }
    }
}
