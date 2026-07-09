using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LanguageLearner_Users_UserId",
                table: "LanguageLearner");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LanguageLearner",
                table: "LanguageLearner");

            migrationBuilder.RenameTable(
                name: "LanguageLearner",
                newName: "LanguageLearners");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LanguageLearners",
                table: "LanguageLearners",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Error = table.Column<string>(type: "text", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessages_ProcessedAt_OccurredAt",
                table: "OutboxMessages",
                columns: new[] { "ProcessedAt", "OccurredAt" });

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageLearners_Users_UserId",
                table: "LanguageLearners",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LanguageLearners_Users_UserId",
                table: "LanguageLearners");

            migrationBuilder.DropTable(
                name: "OutboxMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LanguageLearners",
                table: "LanguageLearners");

            migrationBuilder.RenameTable(
                name: "LanguageLearners",
                newName: "LanguageLearner");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LanguageLearner",
                table: "LanguageLearner",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageLearner_Users_UserId",
                table: "LanguageLearner",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
