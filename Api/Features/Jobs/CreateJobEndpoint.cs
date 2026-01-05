using Application.Abstractions;
using Application.Jobs.CreateJob;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Jobs;

public static class CreateJobEndpoint
{
    public static RouteGroupBuilder MapCreateJob(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (
            [FromBody] CreateJob.Request request,
            IJobsRepository jobs,
            IUnitOfWork uow,
            HttpContext http,
            CancellationToken ct) =>
        {
            var result = await CreateJob.HandleAsync(request, jobs, uow, ct);
            return Results.Created($"/api/jobs/{result.Id}", result);
        })
        .WithName("CreateJob")
        ;

        return group;
    }
}
