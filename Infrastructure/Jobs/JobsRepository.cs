using Application.Abstractions;
using Domain.Jobs;
using Domain.Users;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Jobs;

public sealed class JobsRepository : IJobsRepository
{
    private readonly AppDbContext _db;

    public JobsRepository(AppDbContext db) => _db = db;

    public Task<Job?> GetByIdAsync(Guid id, CancellationToken ct) =>
        _db.Jobs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<Job?> GetByNameAsync(string name, CancellationToken ct) =>
        _db.Jobs.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name, ct);

    public Task AddAsync(Job job, CancellationToken ct) =>
        _db.Jobs.AddAsync(job, ct).AsTask();
}
