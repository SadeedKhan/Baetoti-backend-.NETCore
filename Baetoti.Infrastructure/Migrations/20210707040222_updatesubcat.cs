using Microsoft.EntityFrameworkCore.Migrations;

namespace Baetoti.Infrastructure.Migrations
{
    public partial class updatesubcat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubCategoryArabaciName",
                schema: "baetoti",
                table: "SubCategories",
                newName: "SubCategoryArabicName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubCategoryArabicName",
                schema: "baetoti",
                table: "SubCategories",
                newName: "SubCategoryArabaciName");
        }
    }
}
