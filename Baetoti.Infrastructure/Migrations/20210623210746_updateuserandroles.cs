using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Baetoti.Infrastructure.Migrations
{
    public partial class updateuserandroles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "baetoti",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "baetoti",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                schema: "baetoti",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DOB",
                schema: "baetoti",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentID",
                schema: "baetoti",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DesignationID",
                schema: "baetoti",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "baetoti",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenderID",
                schema: "baetoti",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoiningDate",
                schema: "baetoti",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationID",
                schema: "baetoti",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                schema: "baetoti",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportTo",
                schema: "baetoti",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoleID",
                schema: "baetoti",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShiftID",
                schema: "baetoti",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                schema: "baetoti",
                table: "User",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DOB",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DesignationID",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "GenderID",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "JoiningDate",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LocationID",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Phone",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ReportTo",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RoleID",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ShiftID",
                schema: "baetoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "baetoti",
                table: "User");
        }
    }
}
