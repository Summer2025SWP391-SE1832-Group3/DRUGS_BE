using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class updateSurveyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
        name: "QuestionId",
        table: "SurveyAnswerResults",
        nullable: true); // Nếu cột này có thể null

            // Thêm khóa ngoại liên kết với bảng SurveyQuestions
            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswerResults_SurveyQuestions_QuestionId",
                table: "SurveyAnswerResults",
                column: "QuestionId",
                principalTable: "SurveyQuestions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
          name: "FK_SurveyAnswerResults_SurveyQuestions_QuestionId",
          table: "SurveyAnswerResults");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "SurveyAnswerResults");
        }
    }
}
