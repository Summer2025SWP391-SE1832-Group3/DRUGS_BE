using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1427982a-831d-4ff0-a373-9bed24b4937a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1d81ba52-27b6-48e4-824d-8c722939c77e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7d492a82-2ce8-4014-8b03-a727746c9bcf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd5b8ab9-496c-44ed-8816-81ee0ea10a6d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c574416b-b4c6-4150-bc22-a8eaf0232795");

            migrationBuilder.DropColumn(
                name: "GoogleMeetLink",
                table: "ConsultationRequests");

            migrationBuilder.AddColumn<string>(
                name: "GoogleMeetLink",
                table: "ConsultationSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ConsultantWorkingHours",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConsultationRequestId",
                table: "ConsultantWorkingHours",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ConsultantWorkingHours",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "SlotDate",
                table: "ConsultantWorkingHours",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ConsultantWorkingHours",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ConsultantWorkingHours",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0d86e815-eff1-4291-ba7d-f1188de9e65d", null, "Member", "MEMBER" },
                    { "2c3a35c8-1d5c-4be1-a968-ee99421608a4", null, "Manager", "MANAGER" },
                    { "9aff5d26-3499-454d-b896-c2009d2431bd", null, "Consultant", "CONSULTANT" },
                    { "b9248aed-e127-4044-92f0-fea89b173487", null, "Staff", "STAFF" },
                    { "dbe16734-3ec5-44ec-af79-74aa1d972e10", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsultantWorkingHours_ApplicationUserId",
                table: "ConsultantWorkingHours",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultantWorkingHours_ConsultationRequestId",
                table: "ConsultantWorkingHours",
                column: "ConsultationRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultantWorkingHours_AspNetUsers_ApplicationUserId",
                table: "ConsultantWorkingHours",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultantWorkingHours_ConsultationRequests_ConsultationRequestId",
                table: "ConsultantWorkingHours",
                column: "ConsultationRequestId",
                principalTable: "ConsultationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsultantWorkingHours_AspNetUsers_ApplicationUserId",
                table: "ConsultantWorkingHours");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsultantWorkingHours_ConsultationRequests_ConsultationRequestId",
                table: "ConsultantWorkingHours");

            migrationBuilder.DropIndex(
                name: "IX_ConsultantWorkingHours_ApplicationUserId",
                table: "ConsultantWorkingHours");

            migrationBuilder.DropIndex(
                name: "IX_ConsultantWorkingHours_ConsultationRequestId",
                table: "ConsultantWorkingHours");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0d86e815-eff1-4291-ba7d-f1188de9e65d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c3a35c8-1d5c-4be1-a968-ee99421608a4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9aff5d26-3499-454d-b896-c2009d2431bd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9248aed-e127-4044-92f0-fea89b173487");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dbe16734-3ec5-44ec-af79-74aa1d972e10");

            migrationBuilder.DropColumn(
                name: "GoogleMeetLink",
                table: "ConsultationSessions");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ConsultantWorkingHours");

            migrationBuilder.DropColumn(
                name: "ConsultationRequestId",
                table: "ConsultantWorkingHours");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ConsultantWorkingHours");

            migrationBuilder.DropColumn(
                name: "SlotDate",
                table: "ConsultantWorkingHours");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ConsultantWorkingHours");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ConsultantWorkingHours");

            migrationBuilder.AddColumn<string>(
                name: "GoogleMeetLink",
                table: "ConsultationRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1427982a-831d-4ff0-a373-9bed24b4937a", null, "Member", "MEMBER" },
                    { "1d81ba52-27b6-48e4-824d-8c722939c77e", null, "Consultant", "CONSULTANT" },
                    { "7d492a82-2ce8-4014-8b03-a727746c9bcf", null, "Staff", "STAFF" },
                    { "bd5b8ab9-496c-44ed-8816-81ee0ea10a6d", null, "Manager", "MANAGER" },
                    { "c574416b-b4c6-4150-bc22-a8eaf0232795", null, "Admin", "ADMIN" }
                });
        }
    }
}
