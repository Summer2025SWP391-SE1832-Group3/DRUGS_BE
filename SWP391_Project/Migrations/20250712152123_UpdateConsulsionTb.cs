using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConsulsionTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

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
        }
    }
}
