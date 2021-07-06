using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Baetoti.Infrastructure.Migrations
{
    public partial class Updateshiftinemployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShiftID",
                schema: "baetoti",
                table: "Employee");

            migrationBuilder.AddColumn<string>(
                name: "AveragePreparationTime",
                schema: "baetoti",
                table: "Item",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProviderID",
                schema: "baetoti",
                table: "Item",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Shift",
                schema: "baetoti",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemReview",
                schema: "baetoti",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemID = table.Column<long>(type: "bigint", nullable: false),
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Review = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MarkAsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemReview", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemReview",
                schema: "baetoti");

            migrationBuilder.DropColumn(
                name: "AveragePreparationTime",
                schema: "baetoti",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "ProviderID",
                schema: "baetoti",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Shift",
                schema: "baetoti",
                table: "Employee");

            migrationBuilder.AddColumn<int>(
                name: "ShiftID",
                schema: "baetoti",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
