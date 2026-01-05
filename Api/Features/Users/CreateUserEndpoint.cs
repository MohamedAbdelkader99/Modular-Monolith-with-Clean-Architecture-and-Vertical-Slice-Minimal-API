using Application.Abstractions;
using Application.Users.CreateUser;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Users;

public static class CreateUserEndpoint
{
    public static RouteGroupBuilder MapCreateUser(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (
            [FromBody] CreateUser.Request request,
            IUsersRepository users,
            IUnitOfWork uow,
            HttpContext http,
            CancellationToken ct) =>
        {
            var result = await CreateUser.HandleAsync(request, users, uow, ct);
            return Results.Created($"/api/users/{result.Id}", result);
        })
        .WithName("CreateUser")
        //.WithOpenApi()
        ;

        return group;
    }
}
