using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWP391_Project.Migrations
{
    /// <inheritdoc />
    public partial class updateCourEnrollTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonProgesses");

            migrationBuilder.DropTable(
                name: "Lessions");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0a49ac77-5e14-474d-9e28-a4b3e7575e23");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7fa2eb48-5e2a-4d32-98c7-84cff2b3d57c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8be4c3b0-7246-45fa-9284-0ea0e9c8836e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "97e4a983-27d8-455f-81b9-bf3917f242c7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fc326226-433d-4a35-8175-d19ac0b62e23");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "CourseEnrollments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessons_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LessonProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseEnrollmentId = table.Column<int>(type: "int", nullable: false),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonProgresses_CourseEnrollments_CourseEnrollmentId",
                        column: x => x.CourseEnrollmentId,
                        principalTable: "CourseEnrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonProgresses_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonProgresses_CourseEnrollmentId",
                table: "LessonProgresses",
                column: "CourseEnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonProgresses_LessonId",
                table: "LessonProgresses",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_CourseId",
                table: "Lessons",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonProgresses");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "02f78fca-1094-4ba9-9fe2-9d97916a5e07");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "12f1e9de-6c21-4b70-b6b7-edbd882b43ae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f627e1b-4413-4c8b-a3fa-411f37ab93cd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9431d884-f57c-492c-8cc9-f1d5e1ebf8f1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d0c69b57-2c53-459a-b7ec-c59d25e37ac9");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "CourseEnrollments");

            migrationBuilder.CreateTable(
                name: "Lessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessions_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LessonProgesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseEnrollmentId = table.Column<int>(type: "int", nullable: false),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonProgesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonProgesses_CourseEnrollments_CourseEnrollmentId",
                        column: x => x.CourseEnrollmentId,
                        principalTable: "CourseEnrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonProgesses_Lessions_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lessions_CourseId",
                table: "Lessions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonProgesses_CourseEnrollmentId",
                table: "LessonProgesses",
                column: "CourseEnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonProgesses_LessonId",
                table: "LessonProgesses",
                column: "LessonId");
        }
    }
}
