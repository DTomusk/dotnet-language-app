using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLanguageStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LearnerLanguageStats",
                columns: table => new
                {
                    LanguageLearnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LanguageCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TotalSubmissions = table.Column<int>(type: "integer", nullable: false),
                    UniqueLemmas = table.Column<int>(type: "integer", nullable: false),
                    StartedLearningAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastSubmissionAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearnerLanguageStats", x => new { x.LanguageLearnerId, x.Id });
                    table.ForeignKey(
                        name: "FK_LearnerLanguageStats_LanguageLearners_LanguageLearnerId",
                        column: x => x.LanguageLearnerId,
                        principalTable: "LanguageLearners",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearnerLanguageStats_LanguageCode",
                table: "LearnerLanguageStats",
                column: "LanguageCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LearnerLanguageStats");
        }
    }
}
