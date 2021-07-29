using Microsoft.EntityFrameworkCore.Migrations;

namespace Baetoti.Infrastructure.Migrations
{
    public partial class _7292021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LongitudeLatitude",
                schema: "baetoti",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "baetoti",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LongitudeLatitude",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "baetoti",
                table: "Order");
        }
    }
}
