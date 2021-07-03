using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Baetoti.Infrastructure.Migrations
{
    public partial class ini : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Units",
                schema: "baetoti",
                table: "Units");

            migrationBuilder.RenameTable(
                name: "Units",
                schema: "baetoti",
                newName: "Unit",
                newSchema: "baetoti");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Unit",
                schema: "baetoti",
                table: "Unit",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "Driver",
                schema: "baetoti",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    DriverStatus = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IDNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IDExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrontSideofIDPic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonalPic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrivingLicensePic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpirayDateofLicense = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleRegistrationPic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryDateofVehicleRegistration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleAuthorizationPic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryDateofVehicleAuthorization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalCheckupPic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryDateofMedicalcheckup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleInsurancePic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryDateofVehicleInsurance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MarkAsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Driver", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Provider",
                schema: "baetoti",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    MaroofID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GovernmentID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GovernmentIDPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProviderStatus = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MarkAsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provider", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Driver",
                schema: "baetoti");

            migrationBuilder.DropTable(
                name: "Provider",
                schema: "baetoti");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Unit",
                schema: "baetoti",
                table: "Unit");

            migrationBuilder.RenameTable(
                name: "Unit",
                schema: "baetoti",
                newName: "Units",
                newSchema: "baetoti");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Units",
                schema: "baetoti",
                table: "Units",
                column: "ID");
        }
    }
}
