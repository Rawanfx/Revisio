using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Revisio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHashFilePastExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileHash",
                table: "PastExams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileHash",
                table: "PastExams");
        }
    }
}
