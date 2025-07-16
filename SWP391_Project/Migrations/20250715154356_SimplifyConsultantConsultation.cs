using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyConsultantConsultation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "280aee5a-cc6f-4e38-a1e3-9c74257230e8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "400eda71-0746-4481-b09d-b7d45800d46c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b7b29921-44be-471c-89c5-5c2875cba655");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cda350e0-9284-4e67-a4b7-19d984a42b0f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2e7fe74-cfc1-484e-8dbd-141aa79a2913");

            // Đã xóa đoạn InsertData vào AspNetRoles để tránh lỗi duplicate key khi update database.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0df97ec0-ff55-4e82-8995-2e8086108a8c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20632735-67e8-4e38-8ae1-0dbc222e4b57");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "898f06dd-1928-4a4a-94db-9d03ef407f66");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f47d8a45-3a35-4677-a6c5-10ecb6b01ad8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6137852-19a1-4561-8cfc-f0de53d36417");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "280aee5a-cc6f-4e38-a1e3-9c74257230e8", null, "Manager", "MANAGER" },
                    { "400eda71-0746-4481-b09d-b7d45800d46c", null, "Member", "MEMBER" },
                    { "b7b29921-44be-471c-89c5-5c2875cba655", null, "Staff", "STAFF" },
                    { "cda350e0-9284-4e67-a4b7-19d984a42b0f", null, "Consultant", "CONSULTANT" },
                    { "e2e7fe74-cfc1-484e-8dbd-141aa79a2913", null, "Admin", "ADMIN" }
                });
        }
    }
}
