using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AllowMultipleAnalysesPerUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LanguageAnalysis_UserId",
                table: "LanguageAnalysis");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageAnalysis_UserId",
                table: "LanguageAnalysis",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LanguageAnalysis_UserId",
                table: "LanguageAnalysis");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageAnalysis_UserId",
                table: "LanguageAnalysis",
                column: "UserId",
                unique: true);
        }
    }
}
