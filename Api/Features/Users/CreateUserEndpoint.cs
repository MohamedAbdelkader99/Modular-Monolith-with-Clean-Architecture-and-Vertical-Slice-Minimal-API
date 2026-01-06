using Application.Abstractions;
using Application.Users.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Users;

public static class CreateUserEndpoint
{
    public static RouteGroupBuilder MapCreateUser(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (
            [FromBody] CreateUserCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/users/{result.Id}", result);
        })
        .WithName("CreateUser");

        return group;
    }
}
