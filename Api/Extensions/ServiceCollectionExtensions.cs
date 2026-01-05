using Api.Configuration;
using Application.Abstractions;
using Infrastructure.Jobs;
using Infrastructure.Persistence;
using Infrastructure.Users;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiOptions(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions<SwaggerOptions>()
                .Bind(config.GetSection(SwaggerOptions.SectionName))
                .ValidateDataAnnotations();

            services.AddOptions<JwtOptions>()
                .Bind(config.GetSection(JwtOptions.SectionName));

            return services;
        }

        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddProblemDetails();
            services.AddEndpointsApiExplorer();

            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
        {
            var cs = config.GetConnectionString("Default");
            if (string.IsNullOrWhiteSpace(cs))
                throw new InvalidOperationException("Missing connection string: ConnectionStrings:Default");

            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(cs));

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IJobsRepository, JobsRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddSwaggerIfEnabled(this IServiceCollection services, IConfiguration config)
        {
            var swagger = config.GetSection(SwaggerOptions.SectionName).Get<SwaggerOptions>() ?? new();

            if (!swagger.Enabled) return services;

            services.AddSwaggerGen(c =>
            {
                // Fix schema name conflicts: Request, Response, etc.
                c.CustomSchemaIds(t => t.FullName!.Replace("+", "."));
            });

            return services;
        }
    }
}
