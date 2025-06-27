using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class MoveGoogleMeetLinkToSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleMeetLink",
                table: "ConsultationRequests");

            migrationBuilder.AddColumn<string>(
                name: "GoogleMeetLink",
                table: "ConsultationSessions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleMeetLink",
                table: "ConsultationSessions");

            migrationBuilder.AddColumn<string>(
                name: "GoogleMeetLink",
                table: "ConsultationRequests",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
