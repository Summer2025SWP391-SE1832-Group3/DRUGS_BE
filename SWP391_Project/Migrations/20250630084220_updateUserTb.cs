using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class updateUserTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "02f78fca-1094-4ba9-9fe2-9d97916a5e07");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "12f1e9de-6c21-4b70-b6b7-edbd882b43ae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f627e1b-4413-4c8b-a3fa-411f37ab93cd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9431d884-f57c-492c-8cc9-f1d5e1ebf8f1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d0c69b57-2c53-459a-b7ec-c59d25e37ac9");

            migrationBuilder.AlterColumn<int>(
                name: "YearsOfExperience",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "YearsOfExperience",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "02f78fca-1094-4ba9-9fe2-9d97916a5e07", null, "Manager", "MANAGER" },
                    { "12f1e9de-6c21-4b70-b6b7-edbd882b43ae", null, "Staff", "STAFF" },
                    { "3f627e1b-4413-4c8b-a3fa-411f37ab93cd", null, "Consultant", "CONSULTANT" },
                    { "9431d884-f57c-492c-8cc9-f1d5e1ebf8f1", null, "Member", "MEMBER" },
                    { "d0c69b57-2c53-459a-b7ec-c59d25e37ac9", null, "Admin", "ADMIN" }
                });
        }
    }
}
