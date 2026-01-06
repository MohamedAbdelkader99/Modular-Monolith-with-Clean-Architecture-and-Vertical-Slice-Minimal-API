using Application.Jobs.GetJob;
using MediatR;

namespace Api.Features.Jobs;

public static class GetJobEndpoint
{
    public static RouteGroupBuilder MapGetJob(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetJobQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetJob");

        return group;
    }
}
