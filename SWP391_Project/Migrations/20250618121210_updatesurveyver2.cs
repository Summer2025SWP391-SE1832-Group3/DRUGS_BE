using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class updatesurveyver2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38cca912-63a2-432a-8def-5c6a54d5e3f9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3a9c2ac9-f24c-4411-8e57-5f995bd1edd7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bfcfa2fe-8712-4c94-b0ba-dd7f55eef1b4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6e6ca9f-9892-4f72-9f95-f9195c84f40b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fcaa94ee-120d-42e8-87a9-e49abfb16970");

            migrationBuilder.DropColumn(
                name: "AnswerType",
                table: "SurveyQuestions");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "29fedd3e-3be3-4af0-9402-ba95b1e7a522", null, "Member", "MEMBER" },
                    { "2f54d6cc-c976-4e67-9c11-27a40d06b017", null, "Manager", "MANAGER" },
                    { "70ebfbb8-4ab4-4aa5-aeb5-3d3e4e78471b", null, "Consultant", "CONSULTANT" },
                    { "de7b42dd-0173-4140-9375-4bec97adee01", null, "Admin", "ADMIN" },
                    { "ea57fa1f-bbdd-40ed-bbee-4b95d31e76a7", null, "Staff", "STAFF" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29fedd3e-3be3-4af0-9402-ba95b1e7a522");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2f54d6cc-c976-4e67-9c11-27a40d06b017");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "70ebfbb8-4ab4-4aa5-aeb5-3d3e4e78471b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "de7b42dd-0173-4140-9375-4bec97adee01");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ea57fa1f-bbdd-40ed-bbee-4b95d31e76a7");

            migrationBuilder.AddColumn<string>(
                name: "AnswerType",
                table: "SurveyQuestions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "38cca912-63a2-432a-8def-5c6a54d5e3f9", null, "Admin", "ADMIN" },
                    { "3a9c2ac9-f24c-4411-8e57-5f995bd1edd7", null, "Member", "MEMBER" },
                    { "bfcfa2fe-8712-4c94-b0ba-dd7f55eef1b4", null, "Consultant", "CONSULTANT" },
                    { "e6e6ca9f-9892-4f72-9f95-f9195c84f40b", null, "Manager", "MANAGER" },
                    { "fcaa94ee-120d-42e8-87a9-e49abfb16970", null, "Staff", "STAFF" }
                });
        }
    }
}
