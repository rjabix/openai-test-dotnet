using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class Third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PortalId",
                table: "Jobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ValidUntil",
                table: "Jobs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "portal",
                table: "Jobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PortalId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ValidUntil",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "portal",
                table: "Jobs");
        }
    }
}
