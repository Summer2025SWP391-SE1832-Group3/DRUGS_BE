using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFullNameField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "26307330-d382-4eef-9999-620c52b5480b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7e4e512a-c360-41f4-9434-6cf7d5bf7d2e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e1d4156-4bf6-40f8-9e6a-ac9217fa8291");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "996fa61d-8876-4ccf-8ca3-dbb63a60c9bc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a2290784-0ccf-4e51-ba7e-1280b1d1b75d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2c4d5b2d-e12b-43c8-8ec9-611ec8db3fcf", null, "Admin", "ADMIN" },
                    { "31e7cdc2-773f-4324-b03f-42be5783cdca", null, "Manager", "MANAGER" },
                    { "9cf53721-8742-45e9-ae1e-5b92cadd7a9d", null, "Staff", "STAFF" },
                    { "bf71eb25-a979-4f54-9d83-a3720b6c03e5", null, "Consultant", "CONSULTANT" },
                    { "d640b0dc-26af-4b17-a2ce-a1195ce24ce2", null, "Member", "MEMBER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c4d5b2d-e12b-43c8-8ec9-611ec8db3fcf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31e7cdc2-773f-4324-b03f-42be5783cdca");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9cf53721-8742-45e9-ae1e-5b92cadd7a9d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bf71eb25-a979-4f54-9d83-a3720b6c03e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d640b0dc-26af-4b17-a2ce-a1195ce24ce2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "26307330-d382-4eef-9999-620c52b5480b", null, "Admin", "ADMIN" },
                    { "7e4e512a-c360-41f4-9434-6cf7d5bf7d2e", null, "Consultant", "CONSULTANT" },
                    { "8e1d4156-4bf6-40f8-9e6a-ac9217fa8291", null, "Manager", "MANAGER" },
                    { "996fa61d-8876-4ccf-8ca3-dbb63a60c9bc", null, "Staff", "STAFF" },
                    { "a2290784-0ccf-4e51-ba7e-1280b1d1b75d", null, "Member", "MEMBER" }
                });
        }
    }
}
