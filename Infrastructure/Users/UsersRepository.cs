using Application.Abstractions;
using Domain.Users;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Users;

public sealed class UsersRepository : IUsersRepository
{
    private readonly AppDbContext _db;

    public UsersRepository(AppDbContext db) => _db = db;

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct) =>
        _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct) =>
        _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email, ct);

    public Task AddAsync(User user, CancellationToken ct) =>
        _db.Users.AddAsync(user, ct).AsTask();
}
