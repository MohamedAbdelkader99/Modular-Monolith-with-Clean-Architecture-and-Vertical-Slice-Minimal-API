using Api.Extensions;
using Api.Features.Jobs;
using Api.Features.Users;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiOptions(builder.Configuration)
    .AddApiServices()
    .AddPersistence(builder.Configuration)
    .AddSwaggerIfEnabled(builder.Configuration);

var app = builder.Build();

app.UseGlobalExceptionHandling();
app.UseSwaggerIfEnabled();

app.MapGet("/wazefa", () =>
{
    Results.Ok(new { status = "ok" });
});
// Users module routes
var users = app.MapGroup("/api/users");
users.MapCreateUser();
users.MapGetUser();
// Jobs module routes
var jobs = app.MapGroup("/api/jobs");
jobs.MapCreateJob();
jobs.MapGetJob();

app.Run();
