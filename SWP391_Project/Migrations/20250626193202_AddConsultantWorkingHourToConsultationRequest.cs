using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultantWorkingHourToConsultationRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConsultantWorkingHourId",
                table: "ConsultationRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationRequests_ConsultantWorkingHourId",
                table: "ConsultationRequests",
                column: "ConsultantWorkingHourId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultationRequests_ConsultantWorkingHours_ConsultantWorkingHourId",
                table: "ConsultationRequests",
                column: "ConsultantWorkingHourId",
                principalTable: "ConsultantWorkingHours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsultationRequests_ConsultantWorkingHours_ConsultantWorkingHourId",
                table: "ConsultationRequests");

            migrationBuilder.DropIndex(
                name: "IX_ConsultationRequests_ConsultantWorkingHourId",
                table: "ConsultationRequests");

            migrationBuilder.DropColumn(
                name: "ConsultantWorkingHourId",
                table: "ConsultationRequests");
        }
    }
}
