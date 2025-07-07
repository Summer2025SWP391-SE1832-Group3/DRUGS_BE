using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseTestSurvey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3ec05b72-92c8-432b-814a-8ad414fcfeda");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50fc3489-146e-4646-bdb0-f193c0cdfb07");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f3f1aca-f556-48c9-a1ff-aafe24a0d621");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f72202c8-001f-4680-8089-686fa7afd69a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f8f0e1c2-6ef2-4f61-956c-d120922eff42");

            migrationBuilder.CreateTable(
                name: "CourseTestSurveys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    SurveyId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTestSurveys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTestSurveys_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTestSurveys_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "SurveyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "52d0e991-8e91-4ad0-b641-328b2dd8d070", null, "Admin", "ADMIN" },
                    { "75314ca5-3260-4d65-87f6-3c70135d4793", null, "Manager", "MANAGER" },
                    { "7752e4fd-cafc-49da-89e0-5e2503543b3d", null, "Consultant", "CONSULTANT" },
                    { "9ce5e954-e9e7-4f9b-99cd-9515b5df1d2f", null, "Staff", "STAFF" },
                    { "ffd919ca-da3c-4da1-a436-bbb7d0da5ac7", null, "Member", "MEMBER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseTestSurveys_CourseId",
                table: "CourseTestSurveys",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTestSurveys_SurveyId",
                table: "CourseTestSurveys",
                column: "SurveyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseTestSurveys");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "52d0e991-8e91-4ad0-b641-328b2dd8d070");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "75314ca5-3260-4d65-87f6-3c70135d4793");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7752e4fd-cafc-49da-89e0-5e2503543b3d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9ce5e954-e9e7-4f9b-99cd-9515b5df1d2f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ffd919ca-da3c-4da1-a436-bbb7d0da5ac7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3ec05b72-92c8-432b-814a-8ad414fcfeda", null, "Consultant", "CONSULTANT" },
                    { "50fc3489-146e-4646-bdb0-f193c0cdfb07", null, "Admin", "ADMIN" },
                    { "8f3f1aca-f556-48c9-a1ff-aafe24a0d621", null, "Manager", "MANAGER" },
                    { "f72202c8-001f-4680-8089-686fa7afd69a", null, "Member", "MEMBER" },
                    { "f8f0e1c2-6ef2-4f61-956c-d120922eff42", null, "Staff", "STAFF" }
                });
        }
    }
}
