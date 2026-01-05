using Application.Abstractions;
using Domain.Jobs;

namespace Application.Jobs.CreateJob;

public static class CreateJob
{
    public sealed record Request(string Name);

    public sealed record Result(Guid Id, string Name, DateTime CreatedAtUtc);

    public static async Task<Result> HandleAsync(
        Request request,
        IJobsRepository jobs,
        IUnitOfWork uow,
        CancellationToken ct)
    {
        // Basic validation (you can swap to FluentValidation later)
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required.");
        
        string jobName = request.Name;
        var existing = await jobs.GetByNameAsync(jobName, ct);
        if (existing is not null)
            throw new InvalidOperationException("Name already exists.");

        var job = new Job(request.Name);

        await jobs.AddAsync(job, ct);
        await uow.SaveChangesAsync(ct);

        return new Result(job.Id, job.Name, job.CreatedAtUtc);
    }
}
