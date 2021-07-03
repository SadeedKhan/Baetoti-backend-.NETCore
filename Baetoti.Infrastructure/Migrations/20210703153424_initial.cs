using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Baetoti.Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPasswordUpdateRequired",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastPasswordChangedDate",
                schema: "baetoti",
                table: "User");

            migrationBuilder.AddColumn<bool>(
                name: "IsProfileCompleted",
                schema: "baetoti",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                schema: "baetoti",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "baetoti",
                table: "OTP",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "baetoti",
                table: "Item",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                schema: "baetoti",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Reviews",
                schema: "baetoti",
                table: "Item",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProfileCompleted",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Picture",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "baetoti",
                table: "OTP");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "baetoti",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Rating",
                schema: "baetoti",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Reviews",
                schema: "baetoti",
                table: "Item");

            migrationBuilder.AddColumn<bool>(
                name: "IsPasswordUpdateRequired",
                schema: "baetoti",
                table: "User",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPasswordChangedDate",
                schema: "baetoti",
                table: "User",
                type: "datetime2",
                nullable: true);
        }
    }
}
