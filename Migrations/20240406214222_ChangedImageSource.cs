using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampGroupPlanner.Migrations
{
    /// <inheritdoc />
    public partial class ChangedImageSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaceImages");

            migrationBuilder.AddColumn<string>(
                name: "ImageLink",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageLink",
                table: "Articles");

            migrationBuilder.CreateTable(
                name: "PlaceImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaceImages_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaceImages_ArticleId",
                table: "PlaceImages",
                column: "ArticleId");
        }
    }
}
