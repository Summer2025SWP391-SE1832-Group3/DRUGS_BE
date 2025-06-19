using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class updateDbsetsurvey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswer_SurveyQuestion_QuestionId",
                table: "SurveyAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswerResult_SurveyAnswer_AnswerId",
                table: "SurveyAnswerResult");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswerResult_SurveyResult_SurveyResultId",
                table: "SurveyAnswerResult");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyQuestion_Survey_SurveyId",
                table: "SurveyQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResult_AspNetUsers_UserId",
                table: "SurveyResult");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResult_Survey_SurveyId",
                table: "SurveyResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyResult",
                table: "SurveyResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyQuestion",
                table: "SurveyQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyAnswerResult",
                table: "SurveyAnswerResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyAnswer",
                table: "SurveyAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Survey",
                table: "Survey");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14fbacb5-7c51-46e8-957d-0613f449fc31");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45989231-af64-46b4-9c62-e49d275e5e2a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a90666a9-a398-4069-84f7-36a8344e37c0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eb14a7c0-e94b-452e-b8fc-9518d22e7926");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f45bfa7b-4483-44ea-82c6-37e5f3ce3e2e");

            migrationBuilder.RenameTable(
                name: "SurveyResult",
                newName: "SurveyResults");

            migrationBuilder.RenameTable(
                name: "SurveyQuestion",
                newName: "SurveyQuestions");

            migrationBuilder.RenameTable(
                name: "SurveyAnswerResult",
                newName: "SurveyAnswerResults");

            migrationBuilder.RenameTable(
                name: "SurveyAnswer",
                newName: "SurveyAnswers");

            migrationBuilder.RenameTable(
                name: "Survey",
                newName: "Surveys");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyResult_UserId",
                table: "SurveyResults",
                newName: "IX_SurveyResults_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyResult_SurveyId",
                table: "SurveyResults",
                newName: "IX_SurveyResults_SurveyId");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyQuestion_SurveyId",
                table: "SurveyQuestions",
                newName: "IX_SurveyQuestions_SurveyId");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyAnswerResult_AnswerId",
                table: "SurveyAnswerResults",
                newName: "IX_SurveyAnswerResults_AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyAnswer_QuestionId",
                table: "SurveyAnswers",
                newName: "IX_SurveyAnswers_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyResults",
                table: "SurveyResults",
                column: "ResultId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyQuestions",
                table: "SurveyQuestions",
                column: "QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyAnswerResults",
                table: "SurveyAnswerResults",
                columns: new[] { "SurveyResultId", "AnswerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyAnswers",
                table: "SurveyAnswers",
                column: "AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Surveys",
                table: "Surveys",
                column: "SurveyId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswerResults_SurveyAnswers_AnswerId",
                table: "SurveyAnswerResults",
                column: "AnswerId",
                principalTable: "SurveyAnswers",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswerResults_SurveyResults_SurveyResultId",
                table: "SurveyAnswerResults",
                column: "SurveyResultId",
                principalTable: "SurveyResults",
                principalColumn: "ResultId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswers_SurveyQuestions_QuestionId",
                table: "SurveyAnswers",
                column: "QuestionId",
                principalTable: "SurveyQuestions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyQuestions_Surveys_SurveyId",
                table: "SurveyQuestions",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "SurveyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResults_AspNetUsers_UserId",
                table: "SurveyResults",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResults_Surveys_SurveyId",
                table: "SurveyResults",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "SurveyId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswerResults_SurveyAnswers_AnswerId",
                table: "SurveyAnswerResults");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswerResults_SurveyResults_SurveyResultId",
                table: "SurveyAnswerResults");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswers_SurveyQuestions_QuestionId",
                table: "SurveyAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyQuestions_Surveys_SurveyId",
                table: "SurveyQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResults_AspNetUsers_UserId",
                table: "SurveyResults");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResults_Surveys_SurveyId",
                table: "SurveyResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Surveys",
                table: "Surveys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyResults",
                table: "SurveyResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyQuestions",
                table: "SurveyQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyAnswers",
                table: "SurveyAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyAnswerResults",
                table: "SurveyAnswerResults");

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

            migrationBuilder.RenameTable(
                name: "Surveys",
                newName: "Survey");

            migrationBuilder.RenameTable(
                name: "SurveyResults",
                newName: "SurveyResult");

            migrationBuilder.RenameTable(
                name: "SurveyQuestions",
                newName: "SurveyQuestion");

            migrationBuilder.RenameTable(
                name: "SurveyAnswers",
                newName: "SurveyAnswer");

            migrationBuilder.RenameTable(
                name: "SurveyAnswerResults",
                newName: "SurveyAnswerResult");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyResults_UserId",
                table: "SurveyResult",
                newName: "IX_SurveyResult_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyResults_SurveyId",
                table: "SurveyResult",
                newName: "IX_SurveyResult_SurveyId");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyQuestions_SurveyId",
                table: "SurveyQuestion",
                newName: "IX_SurveyQuestion_SurveyId");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyAnswers_QuestionId",
                table: "SurveyAnswer",
                newName: "IX_SurveyAnswer_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyAnswerResults_AnswerId",
                table: "SurveyAnswerResult",
                newName: "IX_SurveyAnswerResult_AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Survey",
                table: "Survey",
                column: "SurveyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyResult",
                table: "SurveyResult",
                column: "ResultId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyQuestion",
                table: "SurveyQuestion",
                column: "QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyAnswer",
                table: "SurveyAnswer",
                column: "AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyAnswerResult",
                table: "SurveyAnswerResult",
                columns: new[] { "SurveyResultId", "AnswerId" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "14fbacb5-7c51-46e8-957d-0613f449fc31", null, "Consultant", "CONSULTANT" },
                    { "45989231-af64-46b4-9c62-e49d275e5e2a", null, "Member", "MEMBER" },
                    { "a90666a9-a398-4069-84f7-36a8344e37c0", null, "Admin", "ADMIN" },
                    { "eb14a7c0-e94b-452e-b8fc-9518d22e7926", null, "Staff", "STAFF" },
                    { "f45bfa7b-4483-44ea-82c6-37e5f3ce3e2e", null, "Manager", "MANAGER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswer_SurveyQuestion_QuestionId",
                table: "SurveyAnswer",
                column: "QuestionId",
                principalTable: "SurveyQuestion",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswerResult_SurveyAnswer_AnswerId",
                table: "SurveyAnswerResult",
                column: "AnswerId",
                principalTable: "SurveyAnswer",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswerResult_SurveyResult_SurveyResultId",
                table: "SurveyAnswerResult",
                column: "SurveyResultId",
                principalTable: "SurveyResult",
                principalColumn: "ResultId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyQuestion_Survey_SurveyId",
                table: "SurveyQuestion",
                column: "SurveyId",
                principalTable: "Survey",
                principalColumn: "SurveyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResult_AspNetUsers_UserId",
                table: "SurveyResult",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResult_Survey_SurveyId",
                table: "SurveyResult",
                column: "SurveyId",
                principalTable: "Survey",
                principalColumn: "SurveyId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
