using Api.Configuration;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication UseGlobalExceptionHandling(this WebApplication app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var feature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = feature?.Error;

                    if (exception is null)
                        return;

                    ProblemDetails problem;

                    switch (exception)
                    {
                        // FluentValidation errors (400)
                        case ValidationException validationException:
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;

                            problem = new ValidationProblemDetails(
                                validationException.Errors
                                    .GroupBy(e => e.PropertyName)
                                    .ToDictionary(
                                        g => g.Key,
                                        g => g.Select(e => e.ErrorMessage).ToArray()
                                    ))
                            {
                                Title = "Validation failed",
                                Status = StatusCodes.Status400BadRequest,
                                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                            };
                            break;

                        // Bad request (400)
                        case ArgumentException argException:
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;

                            problem = new ProblemDetails
                            {
                                Title = "Invalid request",
                                Detail = argException.Message,
                                Status = StatusCodes.Status400BadRequest
                            };
                            break;

                        // Conflict (409)
                        case InvalidOperationException invalidOpException:
                            context.Response.StatusCode = StatusCodes.Status409Conflict;

                            problem = new ProblemDetails
                            {
                                Title = "Conflict",
                                Detail = invalidOpException.Message,
                                Status = StatusCodes.Status409Conflict
                            };
                            break;

                        // Unhandled errors (500)
                        default:
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                            problem = new ProblemDetails
                            {
                                Title = "Internal server error",
                                Detail = "An unexpected error occurred.",
                                Status = StatusCodes.Status500InternalServerError
                            };
                            break;
                    }

                    context.Response.ContentType = "application/problem+json";
                    await context.Response.WriteAsJsonAsync(problem);
                });
            });

            return app;
        }

        public static WebApplication UseSwaggerIfEnabled(this WebApplication app)
        {
            var swagger = app.Services.GetRequiredService<IOptions<SwaggerOptions>>().Value;

            if (!swagger.Enabled) return app;

            app.UseSwagger();
            app.UseSwaggerUI();

            if (app.Environment.IsDevelopment())
                app.MapGet("/", () => Results.Redirect("/swagger"));

            return app;
        }
    }
}
