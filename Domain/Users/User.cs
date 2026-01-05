namespace Domain.Users;

public sealed class User
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    private User() { } // EF

    public User(string name, string email)
    {
        SetName(name);
        SetEmail(email);
    }

    public void SetName(string name)
    {
        name = (name ?? "").Trim();
        if (name.Length < 2) throw new ArgumentException("Name must be at least 2 characters.");
        Name = name;
    }

    public void SetEmail(string email)
    {
        email = (email ?? "").Trim();
        if (!email.Contains('@')) throw new ArgumentException("Email is invalid.");
        Email = email.ToLowerInvariant();
    }
}
