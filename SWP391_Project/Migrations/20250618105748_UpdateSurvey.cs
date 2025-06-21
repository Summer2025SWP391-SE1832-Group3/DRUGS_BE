using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSurvey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "68f8c696-639a-402f-963b-8829258758db");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8fab023b-cfdf-4666-8952-a5c24f87f1d5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b569a31b-c898-4bdf-932b-353f0418eaaf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "deb450bd-4828-487b-af6a-ce7cbcfcaac7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee7fc57f-584b-45b3-9d3a-e53d4b1c21ad");

            migrationBuilder.AlterColumn<int>(
                name: "SurveyType",
                table: "Surveys",
                type: "int",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Surveys",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0c6bf5c0-52a7-4cb2-ab7b-eeb590c8f80d", null, "Manager", "MANAGER" },
                    { "2bddc71a-70f6-429a-8ed0-20efad7c9c3a", null, "Staff", "STAFF" },
                    { "4f8708b1-5856-4f59-9daf-5ab2c5f6ea9c", null, "Admin", "ADMIN" },
                    { "a8fe6c0c-23ee-4f75-b8ea-e254b454aa98", null, "Member", "MEMBER" },
                    { "ca9cb786-c61f-4f84-bd10-9568b04d9153", null, "Consultant", "CONSULTANT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0c6bf5c0-52a7-4cb2-ab7b-eeb590c8f80d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2bddc71a-70f6-429a-8ed0-20efad7c9c3a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f8708b1-5856-4f59-9daf-5ab2c5f6ea9c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8fe6c0c-23ee-4f75-b8ea-e254b454aa98");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca9cb786-c61f-4f84-bd10-9568b04d9153");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Surveys");

            migrationBuilder.AlterColumn<string>(
                name: "SurveyType",
                table: "Surveys",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 50);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "68f8c696-639a-402f-963b-8829258758db", null, "Consultant", "CONSULTANT" },
                    { "8fab023b-cfdf-4666-8952-a5c24f87f1d5", null, "Staff", "STAFF" },
                    { "b569a31b-c898-4bdf-932b-353f0418eaaf", null, "Member", "MEMBER" },
                    { "deb450bd-4828-487b-af6a-ce7cbcfcaac7", null, "Manager", "MANAGER" },
                    { "ee7fc57f-584b-45b3-9d3a-e53d4b1c21ad", null, "Admin", "ADMIN" }
                });
        }
    }
}
