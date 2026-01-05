using Application.Users.GetUser;
using MediatR;

namespace Api.Features.Users;

public static class GetUserEndpoint
{
    public static RouteGroupBuilder MapGetUser(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetUserQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetUser");

        return group;
    }
}
