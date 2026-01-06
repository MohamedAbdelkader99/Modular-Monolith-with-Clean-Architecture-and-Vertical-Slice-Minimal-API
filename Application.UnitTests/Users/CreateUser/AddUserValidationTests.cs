using Application.Users.CreateUser;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Users.CreateUser;

public sealed class AddUserValidationTests
{
    private readonly AddUserValidation _validator = new();

    [Fact]
    public void Should_fail_when_name_is_empty()
    {
        var cmd = new CreateUserCommand("", "mohamed@test.com");

        var result = _validator.TestValidate(cmd);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_fail_when_email_is_invalid()
    {
        var cmd = new CreateUserCommand("Mohamed", "not-an-email");

        var result = _validator.TestValidate(cmd);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_pass_when_command_is_valid()
    {
        var cmd = new CreateUserCommand("Mohamed", "mohamed@test.com");

        var result = _validator.TestValidate(cmd);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
