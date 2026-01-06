using Application.Abstractions;
using Application.Jobs.CreateJob;
using Application.Users.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Users;

public static class CreateJobEndpoint
{
    public static RouteGroupBuilder MapCreateJob(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (
            [FromBody] CreateJobCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/jobs/{result.Id}", result);
        })
        .WithName("CreateJob");

        return group;
    }
}
