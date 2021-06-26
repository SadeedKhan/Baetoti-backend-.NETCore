using Microsoft.EntityFrameworkCore.Migrations;

namespace Baetoti.Infrastructure.Migrations
{
    public partial class updatemarkasdelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "Units",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "Tags",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "SubCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "EmployeeRole",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "Employee",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "Designation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "Department",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "Category",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "EmployeeRole");

            migrationBuilder.DropColumn(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "Designation");

            migrationBuilder.DropColumn(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "Category");
        }
    }
}
