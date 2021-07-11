using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Baetoti.Infrastructure.Migrations
{
    public partial class Inii : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cart",
                schema: "baetoti");

            migrationBuilder.DropColumn(
                name: "CartID",
                schema: "baetoti",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ItemID",
                schema: "baetoti",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                schema: "baetoti",
                table: "Order",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "Comments",
                schema: "baetoti",
                table: "Order",
                newName: "NotesForDriver");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDeliveryTime",
                schema: "baetoti",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "baetoti",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                schema: "baetoti",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                schema: "baetoti",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedDeliveryTime",
                schema: "baetoti",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                schema: "baetoti",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "baetoti",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                schema: "baetoti",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DriverOrder",
                schema: "baetoti",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<long>(type: "bigint", nullable: false),
                    DriverID = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverOrder", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                schema: "baetoti",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<long>(type: "bigint", nullable: false),
                    ItemID = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ProviderOrder",
                schema: "baetoti",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<long>(type: "bigint", nullable: false),
                    ProviderID = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderOrder", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverOrder",
                schema: "baetoti");

            migrationBuilder.DropTable(
                name: "OrderItem",
                schema: "baetoti");

            migrationBuilder.DropTable(
                name: "ProviderOrder",
                schema: "baetoti");

            migrationBuilder.DropColumn(
                name: "ActualDeliveryTime",
                schema: "baetoti",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "baetoti",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "baetoti",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                schema: "baetoti",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ExpectedDeliveryTime",
                schema: "baetoti",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                schema: "baetoti",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "baetoti",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "baetoti",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "UserID",
                schema: "baetoti",
                table: "Order",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "NotesForDriver",
                schema: "baetoti",
                table: "Order",
                newName: "Comments");

            migrationBuilder.AddColumn<long>(
                name: "CartID",
                schema: "baetoti",
                table: "Order",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ItemID",
                schema: "baetoti",
                table: "Order",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Cart",
                schema: "baetoti",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActualDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpectedDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NotesForDriver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UserIID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.ID);
                });
        }
    }
}
