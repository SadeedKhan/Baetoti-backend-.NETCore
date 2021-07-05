using Microsoft.EntityFrameworkCore.Migrations;

namespace Baetoti.Infrastructure.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "baetoti",
                table: "Privilege",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1L, "View" },
                    { 2L, "Add" },
                    { 3L, "Edit" },
                    { 4L, "Delete" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Privilege",
                keyColumn: "ID",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Privilege",
                keyColumn: "ID",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Privilege",
                keyColumn: "ID",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Privilege",
                keyColumn: "ID",
                keyValue: 4L);
        }
    }
}
