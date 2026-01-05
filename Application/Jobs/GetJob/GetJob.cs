using Application.Abstractions;

namespace Application.Jobs.GetJob;

public static class GetJob
{
    public sealed record Result(Guid Id, string Name, DateTime CreatedAtUtc);

    public static async Task<Result?> HandleAsync(
        Guid id,
        IJobsRepository jobs,
        CancellationToken ct)
    {
        var job = await jobs.GetByIdAsync(id, ct);
        if (job is null) return null;

        return new Result(job.Id, job.Name, job.CreatedAtUtc);
    }
}
