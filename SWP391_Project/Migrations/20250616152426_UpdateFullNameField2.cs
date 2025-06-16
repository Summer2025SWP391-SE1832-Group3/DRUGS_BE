using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFullNameField2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

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
    }
}
