using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSlotAndConsultationLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0f1e4971-70df-449e-bea2-40ee221b6340");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f238753-ee04-4659-ac13-ebf385994ff4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9cdb3016-04a1-4d4a-8436-dfb99b2ba7c8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d629a534-cc92-4475-9c30-6fd2b844822b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3395ab3-5d40-4d6a-a51d-56c773aaf821");

          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32c7e296-4e4e-43fe-a9cc-6e2e073408c4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4abb9925-fae4-4305-ab0a-36bbd0616ff1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "55c7b8a3-7eb1-4999-887f-9cc50366d139");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "875a06bf-69ca-4af2-82fa-792426d4b55e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87665a29-c900-4826-bafb-872d4a85defd");

            // Đã xóa seed role ở đây để tránh duplicate key khi rollback migration
        }
    }
}
