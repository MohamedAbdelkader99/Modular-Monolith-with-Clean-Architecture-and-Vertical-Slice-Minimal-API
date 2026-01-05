using Api.Extensions;
using Api.Features.Jobs;
using Api.Features.Users;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiOptions(builder.Configuration)
    .AddApiServices()
    .AddPersistence(builder.Configuration)
    .AddSwaggerIfEnabled(builder.Configuration);

builder.Services.AddValidatorsFromAssembly(typeof(Application.AssemblyMarker).Assembly);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyMarker).Assembly);
    cfg.AddOpenBehavior(typeof(Application.Behaviors.ValidationBehavior<,>));
});


var app = builder.Build();

app.UseGlobalExceptionHandling();
app.UseSwaggerIfEnabled();

app.MapGet("/wazefa", () =>
{
    Results.Ok(new { status = "ok" });
});
// Users module routes
app.MapGroup("/api/users").MapCreateUser().MapGetUser();
// Jobs module routes
app.MapGroup("/api/jobs").MapCreateJob().MapGetJob();

app.Run();
