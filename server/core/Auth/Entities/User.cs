namespace Domain.Auth.Entities;

public class User
{
    private string _displayName = string.Empty;

    private User() { } // For EF Core

    public Guid Id { get; private set; }
    public string PasswordHash { get; private set; } = string.Empty;

    public string DisplayName
    {
        get => _displayName;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Display name cannot be empty or whitespace.", nameof(DisplayName));

            _displayName = value;
        }
    }

    public DateTime CreatedAt { get; private set; }

    public static User Create(string displayName, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name cannot be empty or whitespace.", nameof(displayName));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty or whitespace.", nameof(passwordHash));

        return new User
        {
            Id = Guid.NewGuid(),
            DisplayName = displayName,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateDisplayName(string newDisplayName)
    {
        if (string.IsNullOrWhiteSpace(newDisplayName))
            throw new ArgumentException("Display name cannot be empty or whitespace.", nameof(newDisplayName));

        DisplayName = newDisplayName;
    }

    public void UpdatePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("Password hash cannot be empty or whitespace.", nameof(newPasswordHash));

        PasswordHash = newPasswordHash;
    }
}