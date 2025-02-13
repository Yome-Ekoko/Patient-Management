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
            migrationBuilder.CreateTable(
                name: "Patient_Management_ROLE",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    NAME = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NORMALIZED_NAME = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CONCURRENCY_STAMP = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Management_ROLE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    FIRST_NAME = table.Column<string>(type: "TEXT", nullable: true),
                    LAST_NAME = table.Column<string>(type: "TEXT", nullable: true),
                    Gender = table.Column<string>(type: "TEXT", nullable: true),
                    ContactAddress = table.Column<string>(type: "TEXT", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "INTEGER", nullable: false),
                    IS_LOGGED_IN = table.Column<bool>(type: "INTEGER", nullable: false),
                    FailedLoginAttempts = table.Column<int>(type: "INTEGER", nullable: false),
                    DEFAULT_ROLE = table.Column<int>(type: "INTEGER", nullable: false),
                    LAST_LOGIN_TIME = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PatientId = table.Column<string>(type: "TEXT", nullable: true),
                    USER_NAME = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NORMALIZED_USER_NAME = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EMAIL = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NORMALIZED_EMAIL = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EMAIL_CONFIRMED = table.Column<bool>(type: "INTEGER", nullable: false),
                    PASSWORD_HASH = table.Column<string>(type: "TEXT", nullable: true),
                    SECURITY_STAMP = table.Column<string>(type: "TEXT", nullable: true),
                    CONCURRENCY_STAMP = table.Column<string>(type: "TEXT", nullable: true),
                    PHONE_NUMBER = table.Column<string>(type: "TEXT", nullable: true),
                    PHONE_NUMBER_CONFIRMED = table.Column<bool>(type: "INTEGER", nullable: false),
                    TWO_FACTOR_ENABLED = table.Column<bool>(type: "INTEGER", nullable: false),
                    LOCKOUT_END = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LOCKOUT_ENABLED = table.Column<bool>(type: "INTEGER", nullable: false),
                    ACCESS_FAILED_COUNT = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Patient_Management_ROLE_CLAIMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ROLE_ID = table.Column<string>(type: "TEXT", nullable: false),
                    CLAIM_TYPE = table.Column<string>(type: "TEXT", nullable: true),
                    CLAIM_VALUE = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Management_ROLE_CL~", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Patient_Management_ROLE_CL~",
                        column: x => x.ROLE_ID,
                        principalTable: "Patient_Management_ROLE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patient_Management_USER_CLAIMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    USER_ID = table.Column<string>(type: "TEXT", nullable: false),
                    CLAIM_TYPE = table.Column<string>(type: "TEXT", nullable: true),
                    CLAIM_VALUE = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Management_USER_CL~", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Patient_Management_USER_CL~",
                        column: x => x.USER_ID,
                        principalTable: "USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patient_Management_USER_LOGINS",
                columns: table => new
                {
                    LOGIN_PROVIDER = table.Column<string>(type: "TEXT", nullable: false),
                    PROVIDER_KEY = table.Column<string>(type: "TEXT", nullable: false),
                    PROVIDER_DISPLAY_NAME = table.Column<string>(type: "TEXT", nullable: true),
                    USER_ID = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Management_USER_LO~", x => new { x.LOGIN_PROVIDER, x.PROVIDER_KEY });
                    table.ForeignKey(
                        name: "FK_Patient_Management_USER_LO~",
                        column: x => x.USER_ID,
                        principalTable: "USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patient_Management_USER_ROLES",
                columns: table => new
                {
                    USER_ID = table.Column<string>(type: "TEXT", nullable: false),
                    ROLE_ID = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Management_USER_RO~", x => new { x.USER_ID, x.ROLE_ID });
                    table.ForeignKey(
                        name: "FK_Patient_Management_USER_RO~",
                        column: x => x.ROLE_ID,
                        principalTable: "Patient_Management_ROLE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patient_Management_USER_R~1",
                        column: x => x.USER_ID,
                        principalTable: "USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patient_Management_USER_TOKENS",
                columns: table => new
                {
                    USER_ID = table.Column<string>(type: "TEXT", nullable: false),
                    LOGIN_PROVIDER = table.Column<string>(type: "TEXT", nullable: false),
                    NAME = table.Column<string>(type: "TEXT", nullable: false),
                    VALUE = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Management_USER_TO~", x => new { x.USER_ID, x.LOGIN_PROVIDER, x.NAME });
                    table.ForeignKey(
                        name: "FK_Patient_Management_USER_TO~",
                        column: x => x.USER_ID,
                        principalTable: "USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HouseNumber = table.Column<string>(type: "TEXT", nullable: true),
                    StreetName = table.Column<string>(type: "TEXT", nullable: true),
                    Landmark = table.Column<string>(type: "TEXT", nullable: true),
                    CityOrTown = table.Column<string>(type: "TEXT", nullable: true),
                    StateOfOrigin = table.Column<string>(type: "TEXT", nullable: true),
                    NickName = table.Column<string>(type: "TEXT", nullable: true),
                    Lga = table.Column<string>(type: "TEXT", nullable: true),
                    State = table.Column<string>(type: "TEXT", nullable: true),
                    NextOfKin = table.Column<string>(type: "TEXT", nullable: true),
                    Nationality = table.Column<string>(type: "TEXT", nullable: true),
                    NextOfKinPhone = table.Column<string>(type: "TEXT", nullable: true),
                    HomeAddress = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Patients_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PatientRecords",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    PatientId = table.Column<string>(type: "TEXT", nullable: true),
                    Diagnosis = table.Column<string>(type: "TEXT", nullable: true),
                    UnderlyingIllness = table.Column<string>(type: "TEXT", nullable: true),
                    MedicalHistory = table.Column<string>(type: "TEXT", nullable: true),
                    Treatment = table.Column<string>(type: "TEXT", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientRecords", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientRecords_Patients_Pa~",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    PatientId = table.Column<string>(type: "TEXT", nullable: true),
                    DoctorId = table.Column<string>(type: "TEXT", nullable: true),
                    AppointmentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AppointmentTime = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    LastAppointmentDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PatientRecordId = table.Column<string>(type: "TEXT", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Appointments_PatientRecord~",
                        column: x => x.PatientRecordId,
                        principalTable: "PatientRecords",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_Pati~",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Appointments_USER_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "USER",
                        principalColumn: "ID");
                });

            migrationBuilder.InsertData(
                table: "Patient_Management_ROLE",
                columns: new[] { "ID", "CONCURRENCY_STAMP", "NAME", "NORMALIZED_NAME" },
                values: new object[,]
                {
                    { "510057bf-a91a-4398-83e7-58a558ae5edd", "71f781f7-e957-469b-96df-9f2035147a23", "Administrator", "ADMINISTRATOR" },
                    { "76cdb59e-48da-4651-b300-a20e9c08a750", "71f781f7-e957-469b-96df-9f2035147a56", "Patient", "PATIENT" },
                    { "887bf7da-6dbb-4429-b646-dc9f2dda56cc", "71f781f7-e957-469b-96df-9f2035147a87", "Doctor", "DOCTOR" }
                });

            migrationBuilder.InsertData(
                table: "USER",
                columns: new[] { "ID", "ACCESS_FAILED_COUNT", "CONCURRENCY_STAMP", "ContactAddress", "CREATED_AT", "EMAIL", "EMAIL_CONFIRMED", "FailedLoginAttempts", "FIRST_NAME", "Gender", "IS_ACTIVE", "IS_LOGGED_IN", "LAST_LOGIN_TIME", "LAST_NAME", "LOCKOUT_ENABLED", "LOCKOUT_END", "NORMALIZED_EMAIL", "NORMALIZED_USER_NAME", "PASSWORD_HASH", "PatientId", "PHONE_NUMBER", "PHONE_NUMBER_CONFIRMED", "SECURITY_STAMP", "TWO_FACTOR_ENABLED", "UpdatedAt", "USER_NAME", "DEFAULT_ROLE" },
                values: new object[,]
                {
                    { "7cc5cd62-6240-44e5-b44f-bff0ae73342", 0, "71f781f7-e957-469b-96df-9f2035147e45", "", new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Oluwatosin.Shada@ACCESSBANKPLC.com", true, 0, "Oluwatosin", null, true, false, new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shada", false, null, "OLUWATOSIN.SHADA@ACCESSBANKPLC.COM", "SHADAO", "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==", null, null, true, "71f781f7-e957-469b-96df-9f2035147e93", false, new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "shadao", 1 },
                    { "9a6a928b-0e11-4d5d-8a29-b8f04445a30", 0, "71f781f7-e957-469b-96df-9f2035147eb1", "", new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daniel.Makinde@ACCESSBANKPLC.com", true, 0, "Daniel", null, true, false, new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Makinde", false, null, "DANIEL.MAKINDE@ACCESSBANKPLC.COM", "MAKINDED", "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==", null, null, true, "71f781f7-e957-469b-96df-9f2035147eb2", false, new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "makinded", 3 },
                    { "9a6a928b-0e11-4d5d-8a29-b8f04445e72", 0, "71f781f7-e957-469b-96df-9f2035147e98", "", new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thelma.Ohue@ACCESSBANKPLC.com", true, 0, "Thelma", null, true, false, new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ohue", false, null, "THELMA.OHUE@ACCESSBANKPLC.COM", "OHUET", "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==", null, null, true, "71f781f7-e957-469b-96df-9f2035147e37", false, new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "ohuet", 2 }
                });

            migrationBuilder.InsertData(
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
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientRecord~",
                table: "Appointments",
                column: "PatientRecordId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Patient_Management_ROLE",
                column: "NORMALIZED_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patient_Management_ROLE_CL~",
                table: "Patient_Management_ROLE_CLAIMS",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_Management_USER_CL~",
                table: "Patient_Management_USER_CLAIMS",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_Management_USER_LO~",
                table: "Patient_Management_USER_LOGINS",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_Management_USER_RO~",
                table: "Patient_Management_USER_ROLES",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientRecords_PatientId",
                table: "PatientRecords",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UserId",
                table: "Patients",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "USER",
                column: "NORMALIZED_EMAIL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "USER",
                column: "NORMALIZED_USER_NAME",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Patient_Management_ROLE_CLAIMS");

            migrationBuilder.DropTable(
                name: "Patient_Management_USER_CLAIMS");

            migrationBuilder.DropTable(
                name: "Patient_Management_USER_LOGINS");

            migrationBuilder.DropTable(
                name: "Patient_Management_USER_ROLES");

            migrationBuilder.DropTable(
                name: "Patient_Management_USER_TOKENS");

            migrationBuilder.DropTable(
                name: "PatientRecords");

            migrationBuilder.DropTable(
                name: "Patient_Management_ROLE");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "USER");
        }
    }
}
