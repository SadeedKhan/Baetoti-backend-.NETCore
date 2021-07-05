using Microsoft.EntityFrameworkCore.Migrations;

namespace Baetoti.Infrastructure.Migrations
{
    public partial class Updatelocationinemployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationID",
                schema: "baetoti",
                table: "Employee");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "baetoti",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                schema: "baetoti",
                table: "Employee");

            migrationBuilder.AddColumn<int>(
                name: "LocationID",
                schema: "baetoti",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
