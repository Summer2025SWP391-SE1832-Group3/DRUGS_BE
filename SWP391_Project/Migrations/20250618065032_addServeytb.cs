using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class addServeytb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d3608f0-3860-4824-9eaf-20091b6d3c32");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6928a100-8bd9-45f3-b0de-f59f334ad932");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8adbc30c-74fe-4fc8-9427-c6fa96b67d2d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d7ec177-21df-4a7f-be7e-530f13f231f1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a7046498-90d2-4d42-8f6e-58dee4dceac7");

            migrationBuilder.CreateTable(
                name: "Survey",
                columns: table => new
                {
                    SurveyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SurveyType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Survey", x => x.SurveyId);
                });

            migrationBuilder.CreateTable(
                name: "SurveyQuestion",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyId = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    AnswerType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyQuestion", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_SurveyQuestion_Survey_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Survey",
                        principalColumn: "SurveyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyResult",
                columns: table => new
                {
                    ResultId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SurveyId = table.Column<int>(type: "int", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    Recommendation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TakeAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResult", x => x.ResultId);
                    table.ForeignKey(
                        name: "FK_SurveyResult_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SurveyResult_Survey_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Survey",
                        principalColumn: "SurveyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SurveyAnswer",
                columns: table => new
                {
                    AnswerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyAnswer", x => x.AnswerId);
                    table.ForeignKey(
                        name: "FK_SurveyAnswer_SurveyQuestion_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "SurveyQuestion",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyAnswerResult",
                columns: table => new
                {
                    SurveyResultId = table.Column<int>(type: "int", nullable: false),
                    AnswerId = table.Column<int>(type: "int", nullable: false),
                    AnswerScore = table.Column<int>(type: "int", nullable: true),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyAnswerResult", x => new { x.SurveyResultId, x.AnswerId });
                    table.ForeignKey(
                        name: "FK_SurveyAnswerResult_SurveyAnswer_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "SurveyAnswer",
                        principalColumn: "AnswerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurveyAnswerResult_SurveyResult_SurveyResultId",
                        column: x => x.SurveyResultId,
                        principalTable: "SurveyResult",
                        principalColumn: "ResultId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "14fbacb5-7c51-46e8-957d-0613f449fc31", null, "Consultant", "CONSULTANT" },
                    { "45989231-af64-46b4-9c62-e49d275e5e2a", null, "Member", "MEMBER" },
                    { "a90666a9-a398-4069-84f7-36a8344e37c0", null, "Admin", "ADMIN" },
                    { "eb14a7c0-e94b-452e-b8fc-9518d22e7926", null, "Staff", "STAFF" },
                    { "f45bfa7b-4483-44ea-82c6-37e5f3ce3e2e", null, "Manager", "MANAGER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAnswer_QuestionId",
                table: "SurveyAnswer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAnswerResult_AnswerId",
                table: "SurveyAnswerResult",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestion_SurveyId",
                table: "SurveyQuestion",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResult_SurveyId",
                table: "SurveyResult",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResult_UserId",
                table: "SurveyResult",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyAnswerResult");

            migrationBuilder.DropTable(
                name: "SurveyAnswer");

            migrationBuilder.DropTable(
                name: "SurveyResult");

            migrationBuilder.DropTable(
                name: "SurveyQuestion");

            migrationBuilder.DropTable(
                name: "Survey");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14fbacb5-7c51-46e8-957d-0613f449fc31");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45989231-af64-46b4-9c62-e49d275e5e2a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a90666a9-a398-4069-84f7-36a8344e37c0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eb14a7c0-e94b-452e-b8fc-9518d22e7926");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f45bfa7b-4483-44ea-82c6-37e5f3ce3e2e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4d3608f0-3860-4824-9eaf-20091b6d3c32", null, "Admin", "ADMIN" },
                    { "6928a100-8bd9-45f3-b0de-f59f334ad932", null, "Staff", "STAFF" },
                    { "8adbc30c-74fe-4fc8-9427-c6fa96b67d2d", null, "Member", "MEMBER" },
                    { "8d7ec177-21df-4a7f-be7e-530f13f231f1", null, "Consultant", "CONSULTANT" },
                    { "a7046498-90d2-4d42-8f6e-58dee4dceac7", null, "Manager", "MANAGER" }
                });
        }
    }
}
