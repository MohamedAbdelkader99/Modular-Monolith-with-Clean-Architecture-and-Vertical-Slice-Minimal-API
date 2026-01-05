using Application.Abstractions;
using MediatR;

namespace Application.Jobs.GetJob;

public sealed record GetJobQuery(Guid Id) : IRequest<GetJobResult?>;

public sealed record GetJobResult(Guid Id, string Name, DateTime CreatedAtUtc);

public sealed class GetJobHandler : IRequestHandler<GetJobQuery, GetJobResult?>
{
    private readonly IJobsRepository _jobs;

    public GetJobHandler(IJobsRepository jobs) => _jobs = jobs;

    public async Task<GetJobResult?> Handle(GetJobQuery request, CancellationToken ct)
    {
        var job = await _jobs.GetByIdAsync(request.Id, ct);
        if (job is null) return null;

        return new GetJobResult(job.Id, job.Name, job.CreatedAtUtc);
    }
}