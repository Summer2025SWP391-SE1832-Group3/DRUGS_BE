using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "26307330-d382-4eef-9999-620c52b5480b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7e4e512a-c360-41f4-9434-6cf7d5bf7d2e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e1d4156-4bf6-40f8-9e6a-ac9217fa8291");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "996fa61d-8876-4ccf-8ca3-dbb63a60c9bc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a2290784-0ccf-4e51-ba7e-1280b1d1b75d");

            migrationBuilder.CreateTable(
                name: "ConsultationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    GoogleMeetLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConsultantId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsultationRequests_AspNetUsers_ConsultantId",
                        column: x => x.ConsultantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsultationRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConsultationReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ConsultationRequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultationReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsultationReviews_ConsultationRequests_ConsultationRequestId",
                        column: x => x.ConsultationRequestId,
                        principalTable: "ConsultationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConsultationSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SessionNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recommendations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConsultationRequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultationSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsultationSessions_ConsultationRequests_ConsultationRequestId",
                        column: x => x.ConsultationRequestId,
                        principalTable: "ConsultationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "10121eb0-f031-4461-aa2f-616978d3050a", null, "Consultant", "CONSULTANT" },
                    { "14e02134-05ba-4661-8be4-0cf45ada6eb6", null, "Admin", "ADMIN" },
                    { "6cc2ab0c-4ede-43a0-a6dc-f9aeaacaeaa0", null, "Manager", "MANAGER" },
                    { "815160a0-514e-4013-ab5c-8748e17f34ac", null, "Staff", "STAFF" },
                    { "e3d41c1e-3778-4eca-8a46-03e7827067af", null, "Member", "MEMBER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationRequests_ConsultantId",
                table: "ConsultationRequests",
                column: "ConsultantId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationRequests_UserId",
                table: "ConsultationRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationReviews_ConsultationRequestId",
                table: "ConsultationReviews",
                column: "ConsultationRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationSessions_ConsultationRequestId",
                table: "ConsultationSessions",
                column: "ConsultationRequestId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsultationReviews");

            migrationBuilder.DropTable(
                name: "ConsultationSessions");

            migrationBuilder.DropTable(
                name: "ConsultationRequests");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "10121eb0-f031-4461-aa2f-616978d3050a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14e02134-05ba-4661-8be4-0cf45ada6eb6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6cc2ab0c-4ede-43a0-a6dc-f9aeaacaeaa0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "815160a0-514e-4013-ab5c-8748e17f34ac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e3d41c1e-3778-4eca-8a46-03e7827067af");

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
