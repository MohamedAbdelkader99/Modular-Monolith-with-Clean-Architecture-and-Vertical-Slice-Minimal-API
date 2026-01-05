using Application.Abstractions;
using Application.Jobs.GetJob;

namespace Api.Features.Jobs;

public static class GetJobEndpoint
{
	public static RouteGroupBuilder MapGetJob(this RouteGroupBuilder group)
	{
		group.MapGet("/{id:guid}", async (
			Guid id,
			IJobsRepository jobs,
			CancellationToken ct) =>
		{
			var result = await GetJob.HandleAsync(id, jobs, ct);
			return result is null ? Results.NotFound() : Results.Ok(result);
		})
		.WithName("GetJob")
		;

		return group;
	}
}
