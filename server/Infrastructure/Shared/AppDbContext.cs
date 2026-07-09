using Domain.Auth.Entities;
using Domain.LanguagePractice.Entities;
using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Shared;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Submission> Submissions => Set<Submission>();

    public DbSet<LanguageLearner> LanguageLearners => Set<LanguageLearner>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

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

        // Value converter for LanguageCode
        var languageCodeConverter = new ValueConverter<LanguageCode, string>(
            v => v.Value,                    // To database: LanguageCode -> string
            v => LanguageCode.From(v)        // From database: string -> LanguageCode
        );

        var nullableLanguageCodeConverter = new ValueConverter<LanguageCode?, string?>(
            v => v != null ? v.Value : null, // To database: LanguageCode? -> string
            v => v != null ? LanguageCode.From(v) : null // From database: string -> LanguageCode?
        );

        // Configure Submission entity
        modelBuilder.Entity<Submission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LanguageCode)
                .HasConversion(languageCodeConverter)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.Text)
                .IsRequired();
            entity.Property(e => e.CreatedAt)
                .IsRequired();

            // Configure the relationship between Submission and User
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure LanguageLearner entity 
        modelBuilder.Entity<LanguageLearner>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.ActiveLanguage)
                .HasConversion(nullableLanguageCodeConverter)
                .HasMaxLength(10);

            entity.HasOne<User>()
                .WithOne()
                .HasForeignKey<LanguageLearner>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure OutboxMessage entity
        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EventType)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Payload)
                .IsRequired();
            entity.Property(e => e.OccurredAt)
                .IsRequired();
            entity.HasIndex(e => new { e.ProcessedAt, e.OccurredAt });
        });
    }
}
