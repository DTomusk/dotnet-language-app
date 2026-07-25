using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixLanguageCodeConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LearnerLanguageStats_LanguageCode",
                table: "LearnerLanguageStats");

            migrationBuilder.CreateIndex(
                name: "IX_LearnerLanguageStats_LanguageCode_LanguageLearnerId",
                table: "LearnerLanguageStats",
                columns: new[] { "LanguageCode", "LanguageLearnerId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LearnerLanguageStats_LanguageCode_LanguageLearnerId",
                table: "LearnerLanguageStats");

            migrationBuilder.CreateIndex(
                name: "IX_LearnerLanguageStats_LanguageCode",
                table: "LearnerLanguageStats",
                column: "LanguageCode",
                unique: true);
        }
    }
}
