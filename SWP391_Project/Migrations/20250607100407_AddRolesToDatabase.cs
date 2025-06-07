using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "036f7140-6ae6-402b-ad5b-156691ce3cdc", null, "Member", "MEMBER" },
                    { "2aa2bce9-fe00-43dc-9992-2a943539c788", null, "Staff", "STAFF" },
                    { "69b5521c-7941-46c9-ab05-bb6fd3d0129f", null, "Consultant", "CONSULTANT" },
                    { "7ae9ec49-3969-49a2-8c53-81d457972024", null, "Admin", "ADMIN" },
                    { "bc862d23-70ba-419d-92cc-cc974da9a689", null, "Manager", "MANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "036f7140-6ae6-402b-ad5b-156691ce3cdc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2aa2bce9-fe00-43dc-9992-2a943539c788");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69b5521c-7941-46c9-ab05-bb6fd3d0129f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ae9ec49-3969-49a2-8c53-81d457972024");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bc862d23-70ba-419d-92cc-cc974da9a689");
        }
    }
}
