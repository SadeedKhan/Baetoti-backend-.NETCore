using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Baetoti.Infrastructure.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDeliveryTime",
                schema: "baetoti",
                table: "Cart",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                schema: "baetoti",
                table: "Cart",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedDeliveryTime",
                schema: "baetoti",
                table: "Cart",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "baetoti",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualDeliveryTime",
                schema: "baetoti",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                schema: "baetoti",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "ExpectedDeliveryTime",
                schema: "baetoti",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "baetoti",
                table: "Cart");
        }
    }
}
