namespace Domain.Jobs;

public sealed class Job
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    private Job() { } // EF

    public Job(string name)
    {
        SetName(name);
    }

    public void SetName(string name)
    {
        name = (name ?? "").Trim();
        if (name.Length < 2) throw new ArgumentException("Name must be at least 2 characters.");
        Name = name;
    }
}
