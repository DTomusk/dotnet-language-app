using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalysisLemmas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalysisTokens");

            migrationBuilder.CreateTable(
                name: "AnalysisLemmas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    LanguageAnalysisId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisLemmas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysisLemmas_LanguageAnalysis_LanguageAnalysisId",
                        column: x => x.LanguageAnalysisId,
                        principalTable: "LanguageAnalysis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisLemmas_LanguageAnalysisId",
                table: "AnalysisLemmas",
                column: "LanguageAnalysisId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalysisLemmas");

            migrationBuilder.CreateTable(
                name: "AnalysisTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageAnalysisId = table.Column<Guid>(type: "uuid", nullable: false),
                    PartOfSpeech = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysisTokens_LanguageAnalysis_LanguageAnalysisId",
                        column: x => x.LanguageAnalysisId,
                        principalTable: "LanguageAnalysis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisTokens_LanguageAnalysisId",
                table: "AnalysisTokens",
                column: "LanguageAnalysisId");
        }
    }
}
