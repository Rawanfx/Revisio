using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Revisio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxScore",
                table: "ExamSessionAnswers");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "ExamSessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserAnswerOption",
                table: "ExamSessionAnswers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessionAnswers_UserAnswerOption",
                table: "ExamSessionAnswers",
                column: "UserAnswerOption");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSessionAnswers_QuestionOptions_UserAnswerOption",
                table: "ExamSessionAnswers",
                column: "UserAnswerOption",
                principalTable: "QuestionOptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamSessionAnswers_QuestionOptions_UserAnswerOption",
                table: "ExamSessionAnswers");

            migrationBuilder.DropIndex(
                name: "IX_ExamSessionAnswers_UserAnswerOption",
                table: "ExamSessionAnswers");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "ExamSessions");

            migrationBuilder.AlterColumn<string>(
                name: "UserAnswerOption",
                table: "ExamSessionAnswers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxScore",
                table: "ExamSessionAnswers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
