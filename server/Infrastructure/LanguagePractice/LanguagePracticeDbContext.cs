using Domain.LanguagePractice.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.LanguagePractice;

public class LanguagePracticeDbContext : DbContext
{
    public LanguagePracticeDbContext(DbContextOptions<LanguagePracticeDbContext> options) : base(options)
    {
    }

    public DbSet<Submission> Submissions => Set<Submission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Submission entity
        modelBuilder.Entity<Submission>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.LanguageCode)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.Text)
                .IsRequired();
            // Not a foreign key reference as separate db contexts
            entity.Property(e => e.UserID)
                .IsRequired();
        });
    }
}
