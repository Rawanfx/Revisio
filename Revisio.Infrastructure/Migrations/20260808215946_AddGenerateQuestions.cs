using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Revisio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGenerateQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenerationRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false),
                    ExamMode = table.Column<int>(type: "int", nullable: false),
                    EssayQuestionNum = table.Column<int>(type: "int", nullable: false),
                    TrueFalseQuestionNum = table.Column<int>(type: "int", nullable: false),
                    MCQQuestionNum = table.Column<int>(type: "int", nullable: false),
                    EasyQuestionNum = table.Column<int>(type: "int", nullable: false),
                    MediumQuestionNum = table.Column<int>(type: "int", nullable: false),
                    HardQuestionNum = table.Column<int>(type: "int", nullable: false),
                    GenrateExamStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenerationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GenerationRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GenerationRequestLectures",
                columns: table => new
                {
                    GenerationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LectureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenerationRequestLectures", x => new { x.LectureId, x.GenerationRequestId });
                    table.ForeignKey(
                        name: "FK_GenerationRequestLectures_GenerationRequests_GenerationRequestId",
                        column: x => x.GenerationRequestId,
                        principalTable: "GenerationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenerationRequestLectures_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenerationRequestLectures_GenerationRequestId",
                table: "GenerationRequestLectures",
                column: "GenerationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_GenerationRequests_UserId",
                table: "GenerationRequests",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenerationRequestLectures");

            migrationBuilder.DropTable(
                name: "GenerationRequests");
        }
    }
}
