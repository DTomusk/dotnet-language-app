using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameUserIdsAndAddCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Users_UserID",
                table: "Submissions");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Submissions",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Submissions",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Submissions_UserID",
                table: "Submissions",
                newName: "IX_Submissions_UserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Submissions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Users_UserId",
                table: "Submissions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Users_UserId",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Submissions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Submissions",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Submissions",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Submissions_UserId",
                table: "Submissions",
                newName: "IX_Submissions_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Users_UserID",
                table: "Submissions",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
