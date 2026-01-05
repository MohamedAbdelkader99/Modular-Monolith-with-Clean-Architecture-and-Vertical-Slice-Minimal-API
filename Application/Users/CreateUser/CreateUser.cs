using Application.Abstractions;
using Domain.Users;
using MediatR;

namespace Application.Users.CreateUser;

public sealed record CreateUserCommand(string Name, string Email)
    : IRequest<CreateUserResult>;

public sealed record CreateUserResult(Guid Id, string Name, string Email, DateTime CreatedAtUtc);

public sealed class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserResult>
{
    private readonly IUsersRepository _users;
    private readonly IUnitOfWork _uow;

    public CreateUserHandler(IUsersRepository users, IUnitOfWork uow)
    {
        _users = users;
        _uow = uow;
    }

    public async Task<CreateUserResult> Handle(CreateUserCommand request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required.");
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email is required.");

        var email = request.Email.Trim().ToLowerInvariant();

        var existing = await _users.GetByEmailAsync(email, ct);
        if (existing is not null)
            throw new InvalidOperationException("Email already exists.");

        var user = new User(request.Name, email);

        await _users.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        return new CreateUserResult(user.Id, user.Name, user.Email, user.CreatedAtUtc);
    }
}
