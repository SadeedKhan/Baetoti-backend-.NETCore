using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Baetoti.Infrastructure.Migrations
{
    public partial class changeitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TempItem",
                schema: "baetoti");

            migrationBuilder.DropTable(
                name: "TempItemTag",
                schema: "baetoti");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "baetoti",
                table: "StoreSchedule",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                schema: "baetoti",
                table: "StoreSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                schema: "baetoti",
                table: "StoreSchedule",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "StoreSchedule",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                schema: "baetoti",
                table: "StoreSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChangeItem",
                schema: "baetoti",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArabicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Reviews = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryID = table.Column<long>(type: "bigint", nullable: false),
                    SubCategoryID = table.Column<long>(type: "bigint", nullable: false),
                    UnitID = table.Column<long>(type: "bigint", nullable: false),
                    ProviderID = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AveragePreparationTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeItem", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ChangeItemTag",
                schema: "baetoti",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemID = table.Column<long>(type: "bigint", nullable: false),
                    TagID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeItemTag", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                schema: "baetoti",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    OrderID = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    TransactionTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangeItem",
                schema: "baetoti");

            migrationBuilder.DropTable(
                name: "ChangeItemTag",
                schema: "baetoti");

            migrationBuilder.DropTable(
                name: "Transaction",
                schema: "baetoti");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "baetoti",
                table: "StoreSchedule");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "baetoti",
                table: "StoreSchedule");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                schema: "baetoti",
                table: "StoreSchedule");

            migrationBuilder.DropColumn(
                name: "MarkAsDeleted",
                schema: "baetoti",
                table: "StoreSchedule");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "baetoti",
                table: "StoreSchedule");

            migrationBuilder.CreateTable(
                name: "TempItem",
                schema: "baetoti",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArabicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AveragePreparationTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryID = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProviderID = table.Column<long>(type: "bigint", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Reviews = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubCategoryID = table.Column<long>(type: "bigint", nullable: false),
                    UnitID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempItem", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TempItemTag",
                schema: "baetoti",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemID = table.Column<long>(type: "bigint", nullable: false),
                    TagID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempItemTag", x => x.ID);
                });
        }
    }
}
