using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultantFeedbackAndRecentChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49363975-d085-4cb7-9119-4fa15ea6629a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4aaf1c49-01fb-4faf-9c72-09622a6e7e7e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b14d08e7-92a1-4538-991b-6b55173b9a49");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2e41c61-5cb7-451f-b502-83ab631ec3ae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd59809a-42d7-4d69-a376-1e319938b44f");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "49363975-d085-4cb7-9119-4fa15ea6629a", null, "Consultant", "CONSULTANT" },
                    { "4aaf1c49-01fb-4faf-9c72-09622a6e7e7e", null, "Manager", "MANAGER" },
                    { "b14d08e7-92a1-4538-991b-6b55173b9a49", null, "Member", "MEMBER" },
                    { "c2e41c61-5cb7-451f-b502-83ab631ec3ae", null, "Staff", "STAFF" },
                    { "cd59809a-42d7-4d69-a376-1e319938b44f", null, "Admin", "ADMIN" }
                });
        }
    }
}
