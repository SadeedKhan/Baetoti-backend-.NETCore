using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Baetoti.Infrastructure.Migrations
{
    public partial class _07292021_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OrderPickUpTime",
                schema: "baetoti",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderPickUpTime",
                schema: "baetoti",
                table: "Order");
        }
    }
}
