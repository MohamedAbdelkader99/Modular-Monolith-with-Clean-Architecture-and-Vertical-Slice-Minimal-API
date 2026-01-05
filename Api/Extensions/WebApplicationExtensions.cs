using Api.Configuration;
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
                    var ex = feature?.Error;

                    var (status, title) = ex switch
                    {
                        ArgumentException => (StatusCodes.Status400BadRequest, "Validation error"),
                        InvalidOperationException => (StatusCodes.Status409Conflict, "Conflict"),
                        _ => (StatusCodes.Status500InternalServerError, "Server error")
                    };

                    context.Response.StatusCode = status;

                    var problem = new ProblemDetails
                    {
                        Status = status,
                        Title = title,
                        Detail = ex?.Message
                    };

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

            // default route -> swagger (dev-friendly)
            if (app.Environment.IsDevelopment())
                app.MapGet("/", () => Results.Redirect("/swagger"));

            return app;
        }
    }
}
