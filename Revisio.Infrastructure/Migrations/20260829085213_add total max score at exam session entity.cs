using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Revisio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addtotalmaxscoreatexamsessionentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExamSessions_GenerationRequestId",
                table: "ExamSessions");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalMaxScore",
                table: "ExamSessions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessions_GenerationRequestId",
                table: "ExamSessions",
                column: "GenerationRequestId"
               // unique: true
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExamSessions_GenerationRequestId",
                table: "ExamSessions");

            migrationBuilder.DropColumn(
                name: "TotalMaxScore",
                table: "ExamSessions");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessions_GenerationRequestId",
                table: "ExamSessions",
                column: "GenerationRequestId");
        }
    }
}
