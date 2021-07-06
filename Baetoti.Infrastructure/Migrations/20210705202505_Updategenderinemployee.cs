using Microsoft.EntityFrameworkCore.Migrations;

namespace Baetoti.Infrastructure.Migrations
{
    public partial class Updategenderinemployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenderID",
                schema: "baetoti",
                table: "Employee");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                schema: "baetoti",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                schema: "baetoti",
                table: "Employee");

            migrationBuilder.AddColumn<int>(
                name: "GenderID",
                schema: "baetoti",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
