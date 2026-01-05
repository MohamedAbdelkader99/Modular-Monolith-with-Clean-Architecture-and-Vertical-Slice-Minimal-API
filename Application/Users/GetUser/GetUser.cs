using Application.Abstractions;

namespace Application.Users.GetUser;

public static class GetUser
{
    public sealed record Result(Guid Id, string Name, string Email, DateTime CreatedAtUtc);

    public static async Task<Result?> HandleAsync(
        Guid id,
        IUsersRepository users,
        CancellationToken ct)
    {
        var user = await users.GetByIdAsync(id, ct);
        if (user is null) return null;

        return new Result(user.Id, user.Name, user.Email, user.CreatedAtUtc);
    }
}
