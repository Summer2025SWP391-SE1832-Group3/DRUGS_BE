﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class InitConsultantAndConsultationAll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsultantWorkingHours_ConsultationRequests_ConsultationRequestId",
                table: "ConsultantWorkingHours");

            migrationBuilder.DropIndex(
                name: "IX_ConsultantWorkingHours_ConsultationRequestId",
                table: "ConsultantWorkingHours");

            migrationBuilder.DropColumn(
                name: "ConsultationRequestId",
                table: "ConsultantWorkingHours");

            migrationBuilder.DropColumn(
                name: "YearsOfExperience",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Feedbacks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ConsultantFeedbacks",
                columns: table => new
                {
                    ConsultantFeedbackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConsultantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultantFeedbacks", x => x.ConsultantFeedbackId);
                    table.ForeignKey(
                        name: "FK_ConsultantFeedbacks_AspNetUsers_ConsultantId",
                        column: x => x.ConsultantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsultantFeedbacks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConsultantProfiles",
                columns: table => new
                {
                    ConsultantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AverageRating = table.Column<double>(type: "float", nullable: false),
                    FeedbackCount = table.Column<int>(type: "int", nullable: false),
                    TotalConsultations = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultantProfiles", x => x.ConsultantId);
                    table.ForeignKey(
                        name: "FK_ConsultantProfiles_AspNetUsers_ConsultantId",
                        column: x => x.ConsultantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsultantFeedbacks_ConsultantId",
                table: "ConsultantFeedbacks",
                column: "ConsultantId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultantFeedbacks_UserId",
                table: "ConsultantFeedbacks",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsultantFeedbacks");

            migrationBuilder.DropTable(
                name: "ConsultantProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConsultationRequestId",
                table: "ConsultantWorkingHours",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearsOfExperience",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConsultantWorkingHours_ConsultationRequestId",
                table: "ConsultantWorkingHours",
                column: "ConsultationRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultantWorkingHours_ConsultationRequests_ConsultationRequestId",
                table: "ConsultantWorkingHours",
                column: "ConsultationRequestId",
                principalTable: "ConsultationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
