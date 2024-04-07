using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampGroupPlanner.Migrations
{
    /// <inheritdoc />
    public partial class DeletedContentInArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Localizations_ArticleId",
                table: "Localizations");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "LocalizationId",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Localizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Localizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Localizations_ArticleId",
                table: "Localizations",
                column: "ArticleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Localizations_ArticleId",
                table: "Localizations");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Localizations");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Localizations");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocalizationId",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Localizations_ArticleId",
                table: "Localizations",
                column: "ArticleId",
                unique: true);
        }
    }
}
