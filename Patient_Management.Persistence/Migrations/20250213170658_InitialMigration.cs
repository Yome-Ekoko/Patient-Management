using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Patient_Management.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "WEBSERVE");

            migrationBuilder.CreateTable(
                name: "BASIC_USER",
                schema: "WEBSERVE",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    API_KEY = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    STATUS = table.Column<int>(type: "int", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_BY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UPDATED_AT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BASIC_USER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Patient_Management_ROLE",
                schema: "WEBSERVE",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NORMALIZED_NAME = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CONCURRENCY_STAMP = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Management_ROLE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USER",
                schema: "WEBSERVE",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FIRST_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LAST_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    IS_LOGGED_IN = table.Column<bool>(type: "bit", nullable: false),
                    FailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    DEFAULT_ROLE = table.Column<int>(type: "int", nullable: false),
                    LAST_LOGIN_TIME = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PatientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USER_NAME = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NORMALIZED_USER_NAME = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EMAIL = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NORMALIZED_EMAIL = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EMAIL_CONFIRMED = table.Column<bool>(type: "bit", nullable: false),
                    PASSWORD_HASH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SECURITY_STAMP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CONCURRENCY_STAMP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PHONE_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PHONE_NUMBER_CONFIRMED = table.Column<bool>(type: "bit", nullable: false),
                    TWO_FACTOR_ENABLED = table.Column<bool>(type: "bit", nullable: false),
                    LOCKOUT_END = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LOCKOUT_ENABLED = table.Column<bool>(type: "bit", nullable: false),
                    ACCESS_FAILED_COUNT = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Patient_Management_ROLE_CLAIMS",
                schema: "WEBSERVE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ROLE_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CLAIM_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CLAIM_VALUE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Management_ROLE_CL~", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Patient_Management_ROLE_CL~",
                        column: x => x.ROLE_ID,
                        principalSchema: "WEBSERVE",
                        principalTable: "Patient_Management_ROLE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patient_Management_USER_CLAIMS",
                schema: "WEBSERVE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USER_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CLAIM_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CLAIM_VALUE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Management_USER_CL~", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Patient_Management_USER_CL~",
                        column: x => x.USER_ID,
                        principalSchema: "WEBSERVE",
                        principalTable: "USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patient_Management_USER_LOGINS",
                schema: "WEBSERVE",
                columns: table => new
                {
                    LOGIN_PROVIDER = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PROVIDER_KEY = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PROVIDER_DISPLAY_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USER_ID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Management_USER_LO~", x => new { x.LOGIN_PROVIDER, x.PROVIDER_KEY });
                    table.ForeignKey(
                        name: "FK_Patient_Management_USER_LO~",
                        column: x => x.USER_ID,
                        principalSchema: "WEBSERVE",
                        principalTable: "USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patient_Management_USER_ROLES",
                schema: "WEBSERVE",
                columns: table => new
                {
                    USER_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ROLE_ID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Management_USER_RO~", x => new { x.USER_ID, x.ROLE_ID });
                    table.ForeignKey(
                        name: "FK_Patient_Management_USER_RO~",
                        column: x => x.ROLE_ID,
                        principalSchema: "WEBSERVE",
                        principalTable: "Patient_Management_ROLE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patient_Management_USER_R~1",
                        column: x => x.USER_ID,
                        principalSchema: "WEBSERVE",
                        principalTable: "USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patient_Management_USER_TOKENS",
                schema: "WEBSERVE",
                columns: table => new
                {
                    USER_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LOGIN_PROVIDER = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VALUE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Management_USER_TO~", x => new { x.USER_ID, x.LOGIN_PROVIDER, x.NAME });
                    table.ForeignKey(
                        name: "FK_Patient_Management_USER_TO~",
                        column: x => x.USER_ID,
                        principalSchema: "WEBSERVE",
                        principalTable: "USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                schema: "WEBSERVE",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Landmark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityOrTown = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateOfOrigin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NickName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lga = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NextOfKin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NextOfKinPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Patients_USER_UserId",
                        column: x => x.UserId,
                        principalSchema: "WEBSERVE",
                        principalTable: "USER",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PatientRecords",
                schema: "WEBSERVE",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnderlyingIllness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treatment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientRecords", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientRecords_Patients_Pa~",
                        column: x => x.PatientId,
                        principalSchema: "WEBSERVE",
                        principalTable: "Patients",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                schema: "WEBSERVE",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastAppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PatientRecordId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Appointments_PatientRecord~",
                        column: x => x.PatientRecordId,
                        principalSchema: "WEBSERVE",
                        principalTable: "PatientRecords",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_Pati~",
                        column: x => x.PatientId,
                        principalSchema: "WEBSERVE",
                        principalTable: "Patients",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Appointments_USER_DoctorId",
                        column: x => x.DoctorId,
                        principalSchema: "WEBSERVE",
                        principalTable: "USER",
                        principalColumn: "ID");
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

            migrationBuilder.InsertData(
                schema: "WEBSERVE",
                table: "Patient_Management_ROLE",
                columns: new[] { "ID", "CONCURRENCY_STAMP", "NAME", "NORMALIZED_NAME" },
                values: new object[,]
                {
                    { "510057bf-a91a-4398-83e7-58a558ae5edd", "71f781f7-e957-469b-96df-9f2035147a23", "Administrator", "ADMINISTRATOR" },
                    { "76cdb59e-48da-4651-b300-a20e9c08a750", "71f781f7-e957-469b-96df-9f2035147a56", "BankStaff", "BANKSTAFF" },
                    { "887bf7da-6dbb-4429-b646-dc9f2dda56cc", "71f781f7-e957-469b-96df-9f2035147a87", "Settlement", "SETTLEMENT" }
                });

            migrationBuilder.InsertData(
                schema: "WEBSERVE",
                table: "USER",
                columns: new[] { "ID", "ACCESS_FAILED_COUNT", "CONCURRENCY_STAMP", "ContactAddress", "CREATED_AT", "EMAIL", "EMAIL_CONFIRMED", "FailedLoginAttempts", "FIRST_NAME", "Gender", "IS_ACTIVE", "IS_LOGGED_IN", "LAST_LOGIN_TIME", "LAST_NAME", "LOCKOUT_ENABLED", "LOCKOUT_END", "NORMALIZED_EMAIL", "NORMALIZED_USER_NAME", "PASSWORD_HASH", "PatientId", "PHONE_NUMBER", "PHONE_NUMBER_CONFIRMED", "SECURITY_STAMP", "TWO_FACTOR_ENABLED", "UpdatedAt", "USER_NAME", "DEFAULT_ROLE" },
                values: new object[,]
                {
                    { "7cc5cd62-6240-44e5-b44f-bff0ae73342", 0, "71f781f7-e957-469b-96df-9f2035147e45", "", new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Oluwatosin.Shada@ACCESSBANKPLC.com", true, 0, "Oluwatosin", null, true, false, new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shada", false, null, "OLUWATOSIN.SHADA@ACCESSBANKPLC.COM", "SHADAO", "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==", null, null, true, "71f781f7-e957-469b-96df-9f2035147e93", false, new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "shadao", 1 },
                    { "9a6a928b-0e11-4d5d-8a29-b8f04445a30", 0, "71f781f7-e957-469b-96df-9f2035147eb1", "", new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daniel.Makinde@ACCESSBANKPLC.com", true, 0, "Daniel", null, true, false, new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Makinde", false, null, "DANIEL.MAKINDE@ACCESSBANKPLC.COM", "MAKINDED", "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==", null, null, true, "71f781f7-e957-469b-96df-9f2035147eb2", false, new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "makinded", 2 },
                    { "9a6a928b-0e11-4d5d-8a29-b8f04445e72", 0, "71f781f7-e957-469b-96df-9f2035147e98", "", new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thelma.Ohue@ACCESSBANKPLC.com", true, 0, "Thelma", null, true, false, new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ohue", false, null, "THELMA.OHUE@ACCESSBANKPLC.COM", "OHUET", "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==", null, null, true, "71f781f7-e957-469b-96df-9f2035147e37", false, new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "ohuet", 2 }
                });

            migrationBuilder.InsertData(
                schema: "WEBSERVE",
                table: "Patient_Management_USER_ROLES",
                columns: new[] { "ROLE_ID", "USER_ID" },
                values: new object[,]
                {
                    { "510057bf-a91a-4398-83e7-58a558ae5edd", "7cc5cd62-6240-44e5-b44f-bff0ae73342" },
                    { "887bf7da-6dbb-4429-b646-dc9f2dda56cc", "9a6a928b-0e11-4d5d-8a29-b8f04445a30" },
                    { "76cdb59e-48da-4651-b300-a20e9c08a750", "9a6a928b-0e11-4d5d-8a29-b8f04445e72" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                schema: "WEBSERVE",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                schema: "WEBSERVE",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientRecord~",
                schema: "WEBSERVE",
                table: "Appointments",
                column: "PatientRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_BASIC_USER_API_KEY",
                schema: "WEBSERVE",
                table: "BASIC_USER",
                column: "API_KEY",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "WEBSERVE",
                table: "Patient_Management_ROLE",
                column: "NORMALIZED_NAME",
                unique: true,
                filter: "[NORMALIZED_NAME] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_Management_ROLE_CL~",
                schema: "WEBSERVE",
                table: "Patient_Management_ROLE_CLAIMS",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_Management_USER_CL~",
                schema: "WEBSERVE",
                table: "Patient_Management_USER_CLAIMS",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_Management_USER_LO~",
                schema: "WEBSERVE",
                table: "Patient_Management_USER_LOGINS",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_Management_USER_RO~",
                schema: "WEBSERVE",
                table: "Patient_Management_USER_ROLES",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientRecords_PatientId",
                schema: "WEBSERVE",
                table: "PatientRecords",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UserId",
                schema: "WEBSERVE",
                table: "Patients",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "WEBSERVE",
                table: "USER",
                column: "NORMALIZED_EMAIL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "WEBSERVE",
                table: "USER",
                column: "NORMALIZED_USER_NAME",
                unique: true,
                filter: "[NORMALIZED_USER_NAME] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments",
                schema: "WEBSERVE");

            migrationBuilder.DropTable(
                name: "BASIC_USER",
                schema: "WEBSERVE");

            migrationBuilder.DropTable(
                name: "Patient_Management_ROLE_CLAIMS",
                schema: "WEBSERVE");

            migrationBuilder.DropTable(
                name: "Patient_Management_USER_CLAIMS",
                schema: "WEBSERVE");

            migrationBuilder.DropTable(
                name: "Patient_Management_USER_LOGINS",
                schema: "WEBSERVE");

            migrationBuilder.DropTable(
                name: "Patient_Management_USER_ROLES",
                schema: "WEBSERVE");

            migrationBuilder.DropTable(
                name: "Patient_Management_USER_TOKENS",
                schema: "WEBSERVE");

            migrationBuilder.DropTable(
                name: "PatientRecords",
                schema: "WEBSERVE");

            migrationBuilder.DropTable(
                name: "Patient_Management_ROLE",
                schema: "WEBSERVE");

            migrationBuilder.DropTable(
                name: "Patients",
                schema: "WEBSERVE");

            migrationBuilder.DropTable(
                name: "USER",
                schema: "WEBSERVE");
        }
    }
}
