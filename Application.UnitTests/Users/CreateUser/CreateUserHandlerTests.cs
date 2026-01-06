using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Users.CreateUser;
using Domain.Users;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Application.UnitTests.Users.CreateUser;

public sealed class CreateUserHandlerTests
{
    private readonly IUsersRepository _users = Substitute.For<IUsersRepository>();

    [Fact]
    public async Task Should_throw_conflict_when_email_already_exists()
    {
        // Arrange
        var existingUser = new User("Existing", "mohamed@test.com");
        _users.GetByEmailAsync("mohamed@test.com", Arg.Any<CancellationToken>())
              .Returns(existingUser);

        var handler = new CreateUserHandler(_users);
        var cmd = new CreateUserCommand("Mohamed", "mohamed@test.com");

        // Act
        var act = async () => await handler.Handle(cmd, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Email already exists*");

        await _users.DidNotReceive().AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Should_add_user_and_return_result_when_email_is_new()
    {
        // Arrange
        _users.GetByEmailAsync("mohamed@test.com", Arg.Any<CancellationToken>())
              .Returns((User?)null);

        var handler = new CreateUserHandler(_users);
        var cmd = new CreateUserCommand("Mohamed", "mohamed@test.com");

        // Act
        var result = await handler.Handle(cmd, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("mohamed@test.com");
        result.Name.Should().Be("Mohamed");

        await _users.Received(1).AddAsync(
            Arg.Is<User>(u => u.Email == "mohamed@test.com" && u.Name == "Mohamed"),
            Arg.Any<CancellationToken>());
    }
}
