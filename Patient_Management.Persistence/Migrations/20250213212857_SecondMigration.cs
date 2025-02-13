using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Patient_Management.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BASIC_USER",
                schema: "WEBSERVE");

            migrationBuilder.RenameTable(
                name: "USER",
                schema: "WEBSERVE",
                newName: "USER");

            migrationBuilder.RenameTable(
                name: "Patients",
                schema: "WEBSERVE",
                newName: "Patients");

            migrationBuilder.RenameTable(
                name: "PatientRecords",
                schema: "WEBSERVE",
                newName: "PatientRecords");

            migrationBuilder.RenameTable(
                name: "Patient_Management_USER_TOKENS",
                schema: "WEBSERVE",
                newName: "Patient_Management_USER_TOKENS");

            migrationBuilder.RenameTable(
                name: "Patient_Management_USER_ROLES",
                schema: "WEBSERVE",
                newName: "Patient_Management_USER_ROLES");

            migrationBuilder.RenameTable(
                name: "Patient_Management_USER_LOGINS",
                schema: "WEBSERVE",
                newName: "Patient_Management_USER_LOGINS");

            migrationBuilder.RenameTable(
                name: "Patient_Management_USER_CLAIMS",
                schema: "WEBSERVE",
                newName: "Patient_Management_USER_CLAIMS");

            migrationBuilder.RenameTable(
                name: "Patient_Management_ROLE_CLAIMS",
                schema: "WEBSERVE",
                newName: "Patient_Management_ROLE_CLAIMS");

            migrationBuilder.RenameTable(
                name: "Patient_Management_ROLE",
                schema: "WEBSERVE",
                newName: "Patient_Management_ROLE");

            migrationBuilder.RenameTable(
                name: "Appointments",
                schema: "WEBSERVE",
                newName: "Appointments");

            migrationBuilder.UpdateData(
                table: "Patient_Management_ROLE",
                keyColumn: "ID",
                keyValue: "76cdb59e-48da-4651-b300-a20e9c08a750",
                columns: new[] { "NAME", "NORMALIZED_NAME" },
                values: new object[] { "Patient", "PATIENT" });

            migrationBuilder.UpdateData(
                table: "Patient_Management_ROLE",
                keyColumn: "ID",
                keyValue: "887bf7da-6dbb-4429-b646-dc9f2dda56cc",
                columns: new[] { "NAME", "NORMALIZED_NAME" },
                values: new object[] { "Doctor", "DOCTOR" });

            migrationBuilder.UpdateData(
                table: "USER",
                keyColumn: "ID",
                keyValue: "9a6a928b-0e11-4d5d-8a29-b8f04445a30",
                column: "DEFAULT_ROLE",
                value: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "WEBSERVE");

            migrationBuilder.RenameTable(
                name: "USER",
                newName: "USER",
                newSchema: "WEBSERVE");

            migrationBuilder.RenameTable(
                name: "Patients",
                newName: "Patients",
                newSchema: "WEBSERVE");

            migrationBuilder.RenameTable(
                name: "PatientRecords",
                newName: "PatientRecords",
                newSchema: "WEBSERVE");

            migrationBuilder.RenameTable(
                name: "Patient_Management_USER_TOKENS",
                newName: "Patient_Management_USER_TOKENS",
                newSchema: "WEBSERVE");

            migrationBuilder.RenameTable(
                name: "Patient_Management_USER_ROLES",
                newName: "Patient_Management_USER_ROLES",
                newSchema: "WEBSERVE");

            migrationBuilder.RenameTable(
                name: "Patient_Management_USER_LOGINS",
                newName: "Patient_Management_USER_LOGINS",
                newSchema: "WEBSERVE");

            migrationBuilder.RenameTable(
                name: "Patient_Management_USER_CLAIMS",
                newName: "Patient_Management_USER_CLAIMS",
                newSchema: "WEBSERVE");

            migrationBuilder.RenameTable(
                name: "Patient_Management_ROLE_CLAIMS",
                newName: "Patient_Management_ROLE_CLAIMS",
                newSchema: "WEBSERVE");

            migrationBuilder.RenameTable(
                name: "Patient_Management_ROLE",
                newName: "Patient_Management_ROLE",
                newSchema: "WEBSERVE");

            migrationBuilder.RenameTable(
                name: "Appointments",
                newName: "Appointments",
                newSchema: "WEBSERVE");

            migrationBuilder.CreateTable(
                name: "BASIC_USER",
                schema: "WEBSERVE",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    API_KEY = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_BY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    STATUS = table.Column<int>(type: "int", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BASIC_USER", x => x.ID);
                });

            migrationBuilder.InsertData(
                schema: "WEBSERVE",
                table: "BASIC_USER",
                columns: new[] { "ID", "API_KEY", "CREATED_AT", "CREATED_BY", "NAME", "STATUS", "UPDATED_AT", "UPDATED_BY" },
                values: new object[,]
                {
                    { "BSR_482242804225Y91361313", "SEC_EPDGYJMVUXEGELEZWHBZGDPZHNIKIZWXUTMJHBNMWWMPBYMFOY", new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", "Access Bank Key 1", 1, new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "System" },
                    { "BSR_482242804225Y91908738", "SEC_EPDGYJMVUXEGELEZWHBZGDPZHNIKIZWXUTMJHBNMWWKDHWIWW", new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", "AFF Key 1", 1, new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "System" }
                });

            migrationBuilder.UpdateData(
                schema: "WEBSERVE",
                table: "Patient_Management_ROLE",
                keyColumn: "ID",
                keyValue: "76cdb59e-48da-4651-b300-a20e9c08a750",
                columns: new[] { "NAME", "NORMALIZED_NAME" },
                values: new object[] { "BankStaff", "BANKSTAFF" });

            migrationBuilder.UpdateData(
                schema: "WEBSERVE",
                table: "Patient_Management_ROLE",
                keyColumn: "ID",
                keyValue: "887bf7da-6dbb-4429-b646-dc9f2dda56cc",
                columns: new[] { "NAME", "NORMALIZED_NAME" },
                values: new object[] { "Settlement", "SETTLEMENT" });

            migrationBuilder.UpdateData(
                schema: "WEBSERVE",
                table: "USER",
                keyColumn: "ID",
                keyValue: "9a6a928b-0e11-4d5d-8a29-b8f04445a30",
                column: "DEFAULT_ROLE",
                value: 2);

            migrationBuilder.CreateIndex(
                name: "IX_BASIC_USER_API_KEY",
                schema: "WEBSERVE",
                table: "BASIC_USER",
                column: "API_KEY",
                unique: true);
        }
    }
}
