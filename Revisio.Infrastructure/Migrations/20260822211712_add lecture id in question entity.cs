using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Revisio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addlectureidinquestionentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LectureId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Questions_LectureId",
                table: "Questions",
                column: "LectureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Lectures_LectureId",
                table: "Questions",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Lectures_LectureId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_LectureId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "LectureId",
                table: "Questions");
        }
    }
}
