
using Application.Abstractions;
using Domain.Jobs;
using MediatR;

namespace Application.Jobs.CreateJob;

public sealed record CreateJobCommand(string Name)
    : IRequest<CreateJobResult>;

public sealed record CreateJobResult(Guid Id, string Name, DateTime CreatedAtUtc);

public sealed class CreateJobHandler : IRequestHandler<CreateJobCommand, CreateJobResult>
{
    private readonly IJobsRepository _jobs;

    public CreateJobHandler(IJobsRepository jobs)
    {
        _jobs = jobs;
    }

    public async Task<CreateJobResult> Handle(CreateJobCommand request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required.");

        var name = request.Name.Trim().ToLowerInvariant();

        var existing = await _jobs.GetByNameAsync(name, ct);
        if (existing is not null)
            throw new InvalidOperationException("Name already exists.");

        var job = new Job(name);

        await _jobs.AddAsync(job, ct);

        return new CreateJobResult(job.Id, job.Name ,job.CreatedAtUtc);
    }
}
