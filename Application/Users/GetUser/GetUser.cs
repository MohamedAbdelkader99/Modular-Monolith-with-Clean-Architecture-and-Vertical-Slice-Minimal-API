using Application.Abstractions;
using MediatR;

namespace Application.Users.GetUser;

public sealed record GetUserQuery(Guid Id) : IRequest<GetUserResult?>;

public sealed record GetUserResult(Guid Id, string Name, string Email, DateTime CreatedAtUtc);

public sealed class GetUserHandler : IRequestHandler<GetUserQuery, GetUserResult?>
{
    private readonly IUsersRepository _users;

    public GetUserHandler(IUsersRepository users) => _users = users;

    public async Task<GetUserResult?> Handle(GetUserQuery request, CancellationToken ct)
    {
        var user = await _users.GetByIdAsync(request.Id, ct);
        if (user is null) return null;

        return new GetUserResult(user.Id, user.Name, user.Email, user.CreatedAtUtc);
    }
}