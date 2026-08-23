using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Revisio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addlectureidinquestionentity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Lectures_LectureId",
                table: "Questions");

            migrationBuilder.AlterColumn<Guid>(
                name: "LectureId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Lectures_LectureId",
                table: "Questions",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Lectures_LectureId",
                table: "Questions");

            migrationBuilder.AlterColumn<Guid>(
                name: "LectureId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Lectures_LectureId",
                table: "Questions",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
