using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultantStatsColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Feedbacks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ConsultantId",
                table: "Feedbacks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            // Thêm bảng ConsultantProfile
            migrationBuilder.CreateTable(
                name: "ConsultantProfiles",
                columns: table => new
                {
                    ConsultantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Active"),
                    AverageRating = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    FeedbackCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalConsultations = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultantProfiles", x => x.ConsultantId);
                    table.ForeignKey(
                        name: "FK_ConsultantProfiles_AspNetUsers_ConsultantId",
                        column: x => x.ConsultantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ConsultantId",
                table: "Feedbacks",
                column: "ConsultantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_AspNetUsers_ConsultantId",
                table: "Feedbacks",
                column: "ConsultantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_AspNetUsers_ConsultantId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_ConsultantId",
                table: "Feedbacks");

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

            migrationBuilder.DropColumn(
                name: "ConsultantId",
                table: "Feedbacks");

            // Down: Xóa bảng ConsultantProfile, không xóa cột ở AspNetUsers nữa
            migrationBuilder.DropTable(name: "ConsultantProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Feedbacks",
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
                    { "0df97ec0-ff55-4e82-8995-2e8086108a8c", null, "Staff", "STAFF" },
                    { "20632735-67e8-4e38-8ae1-0dbc222e4b57", null, "Manager", "MANAGER" },
                    { "898f06dd-1928-4a4a-94db-9d03ef407f66", null, "Admin", "ADMIN" },
                    { "f47d8a45-3a35-4677-a6c5-10ecb6b01ad8", null, "Consultant", "CONSULTANT" },
                    { "f6137852-19a1-4561-8cfc-f0de53d36417", null, "Member", "MEMBER" }
                });
        }
    }
}
