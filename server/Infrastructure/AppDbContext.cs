using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Submission> Submissions => Set<Submission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(100);
            entity.HasIndex(e => e.DisplayName)
                .IsUnique();
            entity.Property(e => e.PasswordHash)
                .IsRequired();
            entity.Property(e => e.CreatedAt)
                .IsRequired();
        });

        // Configure Submission entity
        modelBuilder.Entity<Submission>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.LanguageCode)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.Text)
                .IsRequired();

            // Foreign key relationship
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
