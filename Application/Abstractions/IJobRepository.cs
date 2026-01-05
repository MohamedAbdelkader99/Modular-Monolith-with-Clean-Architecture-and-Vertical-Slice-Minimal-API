using Domain.Jobs;

namespace Application.Abstractions;

public interface IJobsRepository
{
    Task<Job?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Job?> GetByNameAsync(string name, CancellationToken ct);
    Task AddAsync(Job job, CancellationToken ct);
}
