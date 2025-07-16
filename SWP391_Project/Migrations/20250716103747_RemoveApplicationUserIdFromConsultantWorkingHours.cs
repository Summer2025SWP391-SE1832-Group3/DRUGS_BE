using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class RemoveApplicationUserIdFromConsultantWorkingHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_AspNetUsers_ConsultantId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_ConsultantId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "ConsultantId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "YearsOfExperience",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConsultantId",
                table: "Feedbacks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "YearsOfExperience",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

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
    }
}
