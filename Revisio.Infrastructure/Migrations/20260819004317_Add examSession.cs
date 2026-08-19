using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Revisio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddexamSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExamSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GenerationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false),
                    CorrectAnswersCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamSessions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamSessions_GenerationRequests_GenerationRequestId",
                        column: x => x.GenerationRequestId,
                        principalTable: "GenerationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExamSessionAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UserAnswerOption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAnswerEsaay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileKeyUpload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeTakeForAnswer = table.Column<TimeSpan>(type: "time", nullable: false),
                    MaxScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExamSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSessionAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamSessionAnswers_ExamSessions_ExamSessionId",
                        column: x => x.ExamSessionId,
                        principalTable: "ExamSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamSessionAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessionAnswers_ExamSessionId",
                table: "ExamSessionAnswers",
                column: "ExamSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessionAnswers_QuestionId",
                table: "ExamSessionAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessions_GenerationRequestId",
                table: "ExamSessions",
                column: "GenerationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessions_UserId",
                table: "ExamSessions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamSessionAnswers");

            migrationBuilder.DropTable(
                name: "ExamSessions");
        }
    }
}
