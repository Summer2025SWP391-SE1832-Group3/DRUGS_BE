using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IssuingOrganization = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DateIssued = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConsultantWorkingHours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConsultantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultantWorkingHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsultantWorkingHours_AspNetUsers_ConsultantId",
                        column: x => x.ConsultantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_ApplicationUserId",
                table: "Certificates",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultantWorkingHours_ConsultantId",
                table: "ConsultantWorkingHours",
                column: "ConsultantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "ConsultantWorkingHours");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "285f5de6-cca3-44e3-a410-7a23c3e0f84b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a27e46d1-e003-41cb-b530-ed3b9a6b6086");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a316c46a-e3f5-4a7c-bd47-04ee27f58a75");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a6c59c81-9667-4eaa-94d3-46582369dbe0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aefafb0a-9cf7-4eb1-adb6-2ad0e5056860");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "26307330-d382-4eef-9999-620c52b5480b", null, "Admin", "ADMIN" },
                    { "7e4e512a-c360-41f4-9434-6cf7d5bf7d2e", null, "Consultant", "CONSULTANT" },
                    { "8e1d4156-4bf6-40f8-9e6a-ac9217fa8291", null, "Manager", "MANAGER" },
                    { "996fa61d-8876-4ccf-8ca3-dbb63a60c9bc", null, "Staff", "STAFF" },
                    { "a2290784-0ccf-4e51-ba7e-1280b1d1b75d", null, "Member", "MEMBER" }
                });
        }
    }
}
