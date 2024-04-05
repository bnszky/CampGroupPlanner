using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampGroupPlanner.Migrations
{
    /// <inheritdoc />
    public partial class AddedSourceLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SourceLink",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceLink",
                table: "Articles");
        }
    }
}
