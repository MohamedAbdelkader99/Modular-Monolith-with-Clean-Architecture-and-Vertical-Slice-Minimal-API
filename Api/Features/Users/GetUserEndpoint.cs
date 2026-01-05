using Application.Abstractions;
using Application.Users.GetUser;

namespace Api.Features.Users;

public static class GetUserEndpoint
{
	public static RouteGroupBuilder MapGetUser(this RouteGroupBuilder group)
	{
		group.MapGet("/{id:guid}", async (
			Guid id,
			IUsersRepository users,
			CancellationToken ct) =>
		{
			var result = await GetUser.HandleAsync(id, users, ct);
			return result is null ? Results.NotFound() : Results.Ok(result);
		})
		.WithName("GetUser")
		;

		return group;
	}
}
