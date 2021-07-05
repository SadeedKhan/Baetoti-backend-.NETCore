using Microsoft.EntityFrameworkCore.Migrations;

namespace Baetoti.Infrastructure.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "baetoti",
                table: "Menu",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1L, "Dashboard" },
                    { 16L, "User Profile" },
                    { 15L, "Profile" },
                    { 13L, "Project Management" },
                    { 12L, "Invoice" },
                    { 11L, "Feature Ads" },
                    { 10L, "Configs" },
                    { 9L, "Notifications" },
                    { 14L, "Complaints" },
                    { 7L, "Transactions" },
                    { 6L, "Orders" },
                    { 5L, "Items" },
                    { 4L, "Users" },
                    { 3L, "Categories" },
                    { 2L, "Staff" },
                    { 8L, "Analytics" }
                });

            migrationBuilder.InsertData(
                schema: "baetoti",
                table: "SubMenu",
                columns: new[] { "ID", "MenuID", "Name" },
                values: new object[,]
                {
                    { 14L, 8L, "Statics" },
                    { 15L, 8L, "Cohort Analysis" },
                    { 16L, 8L, "Revenue" },
                    { 17L, 8L, "Finance" },
                    { 21L, 10L, "VAT" },
                    { 19L, 9L, "Push Notification" },
                    { 20L, 10L, "Commission" },
                    { 22L, 10L, "Driver Config" },
                    { 13L, 8L, "Map" },
                    { 18L, 9L, "Notification" },
                    { 12L, 5L, "Change Item Req." },
                    { 4L, 2L, "Role & Privileges" },
                    { 10L, 4L, "Join Req." },
                    { 9L, 4L, "User List" },
                    { 8L, 3L, "Tags" },
                    { 7L, 3L, "Units" },
                    { 6L, 3L, "Category" },
                    { 5L, 2L, "Last Login" },
                    { 23L, 10L, "Conutry Config" },
                    { 3L, 2L, "Employees" },
                    { 2L, 1L, "Secondary" },
                    { 1L, 1L, "Primary" },
                    { 11L, 5L, "Items List" },
                    { 24L, 10L, "Currency Config" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Menu",
                keyColumn: "ID",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Menu",
                keyColumn: "ID",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Menu",
                keyColumn: "ID",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Menu",
                keyColumn: "ID",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Menu",
                keyColumn: "ID",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Menu",
                keyColumn: "ID",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Menu",
                keyColumn: "ID",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Menu",
                keyColumn: "ID",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Menu",
                keyColumn: "ID",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Menu",
                keyColumn: "ID",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Menu",
                keyColumn: "ID",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Menu",
                keyColumn: "ID",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Menu",
                keyColumn: "ID",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Menu",
                keyColumn: "ID",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Menu",
                keyColumn: "ID",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "Menu",
                keyColumn: "ID",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                schema: "baetoti",
                table: "SubMenu",
                keyColumn: "ID",
                keyValue: 24L);
        }
    }
}
