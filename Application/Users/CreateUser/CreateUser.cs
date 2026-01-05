using Application.Abstractions;
using Domain.Users;

namespace Application.Users.CreateUser;

public static class CreateUser
{
    public sealed record Request(string Name, string Email);

    public sealed record Result(Guid Id, string Name, string Email, DateTime CreatedAtUtc);

    public static async Task<Result> HandleAsync(
        Request request,
        IUsersRepository users,
        IUnitOfWork uow,
        CancellationToken ct)
    {
        // Basic validation (you can swap to FluentValidation later)
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required.");
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email is required.");

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var existing = await users.GetByEmailAsync(normalizedEmail, ct);
        if (existing is not null)
            throw new InvalidOperationException("Email already exists.");

        var user = new User(request.Name, normalizedEmail);

        await users.AddAsync(user, ct);
        await uow.SaveChangesAsync(ct);

        return new Result(user.Id, user.Name, user.Email, user.CreatedAtUtc);
    }
}
