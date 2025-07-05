using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class CheckForChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0d86e815-eff1-4291-ba7d-f1188de9e65d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c3a35c8-1d5c-4be1-a968-ee99421608a4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9aff5d26-3499-454d-b896-c2009d2431bd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9248aed-e127-4044-92f0-fea89b173487");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dbe16734-3ec5-44ec-af79-74aa1d972e10");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0d86e815-eff1-4291-ba7d-f1188de9e65d", null, "Member", "MEMBER" },
                    { "2c3a35c8-1d5c-4be1-a968-ee99421608a4", null, "Manager", "MANAGER" },
                    { "9aff5d26-3499-454d-b896-c2009d2431bd", null, "Consultant", "CONSULTANT" },
                    { "b9248aed-e127-4044-92f0-fea89b173487", null, "Staff", "STAFF" },
                    { "dbe16734-3ec5-44ec-af79-74aa1d972e10", null, "Admin", "ADMIN" }
                });
        }
    }
}
