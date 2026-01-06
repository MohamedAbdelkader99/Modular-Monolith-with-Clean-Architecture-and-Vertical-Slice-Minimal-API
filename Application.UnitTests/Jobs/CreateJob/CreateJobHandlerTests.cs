using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Jobs.CreateJob;
using Domain.Jobs;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Application.UnitTests.Jobs.CreateJob;

public sealed class CreateJobHandlerTests
{
    private readonly IJobsRepository _Jobs = Substitute.For<IJobsRepository>();

    [Fact]
    public async Task Should_throw_conflict_when_email_already_exists()
    {
        // Arrange
        var existingJob = new Job("command");
        _Jobs.GetByNameAsync("command", Arg.Any<CancellationToken>())
              .Returns(existingJob);

        var handler = new CreateJobHandler(_Jobs);
        var cmd = new CreateJobCommand("command");

        // Act
        var act = async () => await handler.Handle(cmd, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Name already exists*");

        await _Jobs.DidNotReceive().AddAsync(Arg.Any<Job>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Should_add_Job_and_return_result_when_name_is_new()
    {
        // Arrange
        _Jobs.GetByNameAsync("command", Arg.Any<CancellationToken>())
              .Returns((Job?)null);

        var handler = new CreateJobHandler(_Jobs);
        var cmd = new CreateJobCommand("command");

        // Act
        var result = await handler.Handle(cmd, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("command");

        await _Jobs.Received(1).AddAsync(
            Arg.Is<Job>(u => u.Name == "command"),
            Arg.Any<CancellationToken>());
    }
}
