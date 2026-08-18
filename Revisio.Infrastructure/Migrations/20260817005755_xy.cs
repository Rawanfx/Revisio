using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Revisio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class xy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_GenerationRequests_GenrationRequestId",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "GenrationRequestId",
                table: "Questions",
                newName: "GenerationRequestId");

            migrationBuilder.RenameColumn(
                name: "Explantion",
                table: "Questions",
                newName: "Explanation");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_GenrationRequestId",
                table: "Questions",
                newName: "IX_Questions_GenerationRequestId");

            migrationBuilder.AddColumn<string>(
                name: "GradingCriteria",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelAnswer",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_GenerationRequests_GenerationRequestId",
                table: "Questions",
                column: "GenerationRequestId",
                principalTable: "GenerationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_GenerationRequests_GenerationRequestId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "GradingCriteria",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ModelAnswer",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "GenerationRequestId",
                table: "Questions",
                newName: "GenrationRequestId");

            migrationBuilder.RenameColumn(
                name: "Explanation",
                table: "Questions",
                newName: "Explantion");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_GenerationRequestId",
                table: "Questions",
                newName: "IX_Questions_GenrationRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_GenerationRequests_GenrationRequestId",
                table: "Questions",
                column: "GenrationRequestId",
                principalTable: "GenerationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
