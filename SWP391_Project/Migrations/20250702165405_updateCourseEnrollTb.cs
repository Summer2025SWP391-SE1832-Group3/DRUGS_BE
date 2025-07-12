using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class updateCourseEnrollTb : Migration
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

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CourseEnrollments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SuspendedUntil",
                table: "CourseEnrollments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0b4b4621-c539-4e7d-917e-be2d9ead3daf", null, "Consultant", "CONSULTANT" },
                    { "2d3cfa58-d11e-4696-88c9-e00ec9aabffe", null, "Admin", "ADMIN" },
                    { "38acae95-9137-4f36-a840-b0dc82ede636", null, "Member", "MEMBER" },
                    { "66820bd0-e4a8-44af-8a2a-f27253b5fd47", null, "Manager", "MANAGER" },
                    { "76d55222-8711-463a-ba61-3091c3bb286d", null, "Staff", "STAFF" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0b4b4621-c539-4e7d-917e-be2d9ead3daf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d3cfa58-d11e-4696-88c9-e00ec9aabffe");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38acae95-9137-4f36-a840-b0dc82ede636");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "66820bd0-e4a8-44af-8a2a-f27253b5fd47");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "76d55222-8711-463a-ba61-3091c3bb286d");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CourseEnrollments");

            migrationBuilder.DropColumn(
                name: "SuspendedUntil",
                table: "CourseEnrollments");

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
