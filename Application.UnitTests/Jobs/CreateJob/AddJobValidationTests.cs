using Application.Jobs.CreateJob;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Jobs.CreateJob;

public sealed class AddJobValidationTests
{
    private readonly AddJobValidation _validator = new();

    [Fact]
    public void Should_fail_when_name_is_empty()
    {
        var cmd = new CreateJobCommand("command");

        var result = _validator.TestValidate(cmd);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }


    [Fact]
    public void Should_pass_when_command_is_valid()
    {
        var cmd = new CreateJobCommand("command");

        var result = _validator.TestValidate(cmd);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
