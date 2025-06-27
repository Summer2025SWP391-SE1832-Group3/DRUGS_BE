using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConsultantWorkingHourAndConsultationReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13196a02-f972-47ae-a6b8-b27fc9e4b299");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4fc827df-84e6-4c78-9d0e-f77bc173fac6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "65108aac-5308-4ae0-97be-d26addecbf38");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3eaca59-7862-4386-a4c3-6bedb60c5497");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb80e183-c9ff-4f92-a291-2db61db93cd1");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "13196a02-f972-47ae-a6b8-b27fc9e4b299", null, "Staff", "STAFF" },
                    { "4fc827df-84e6-4c78-9d0e-f77bc173fac6", null, "Admin", "ADMIN" },
                    { "65108aac-5308-4ae0-97be-d26addecbf38", null, "Manager", "MANAGER" },
                    { "c3eaca59-7862-4386-a4c3-6bedb60c5497", null, "Consultant", "CONSULTANT" },
                    { "fb80e183-c9ff-4f92-a291-2db61db93cd1", null, "Member", "MEMBER" }
                });
        }
    }
}
