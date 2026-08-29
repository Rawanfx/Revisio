using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Revisio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addcourseidingenerationrequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "GenerationRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("4DA8F8A8-A321-4B49-7645-08DEE9E40462"));

            migrationBuilder.CreateIndex(
                name: "IX_GenerationRequests_CourseId",
                table: "GenerationRequests",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_GenerationRequests_Courses_CourseId",
                table: "GenerationRequests",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GenerationRequests_Courses_CourseId",
                table: "GenerationRequests");

            migrationBuilder.DropIndex(
                name: "IX_GenerationRequests_CourseId",
                table: "GenerationRequests");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "GenerationRequests");
        }
    }
}
