using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class updateTableSurvey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "AnswerScore",
            table: "SurveyAnswerResults");

            migrationBuilder.DropColumn(
                name: "AnswerText",
                table: "SurveyAnswerResults");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
           name: "AnswerScore",
           table: "SurveyAnswerResults",
           type: "int",
           nullable: true); 

            migrationBuilder.AddColumn<string>(
                name: "AnswerText",
                table: "SurveyAnswerResults",
                type: "nvarchar(max)",
                nullable: true); 
        }
    }
}
