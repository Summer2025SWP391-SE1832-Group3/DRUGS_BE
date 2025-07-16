using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultantProfileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FeedbackCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TotalConsultations",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ConsultantProfiles",
                columns: table => new
                {
                    ConsultantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AverageRating = table.Column<double>(type: "float", nullable: false),
                    FeedbackCount = table.Column<int>(type: "int", nullable: false),
                    TotalConsultations = table.Column<int>(type: "int", nullable: false)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsultantProfiles");

            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "FeedbackCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalConsultations",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
