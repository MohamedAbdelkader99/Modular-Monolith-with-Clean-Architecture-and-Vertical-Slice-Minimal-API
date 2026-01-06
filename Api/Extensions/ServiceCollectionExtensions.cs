using Api.Configuration;
using Application.Abstractions;
using FluentValidation;
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
            services.AddScoped<Application.Abstractions.IAppDbContext>(sp =>
                    sp.GetRequiredService<Infrastructure.Persistence.AppDbContext>());

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IJobsRepository, JobsRepository>();

            return services;
        }

        public static IServiceCollection AddSwaggerIfEnabled(this IServiceCollection services, IConfiguration config)
        {
            var swagger = config.GetSection(SwaggerOptions.SectionName).Get<SwaggerOptions>() ?? new();

            if (!swagger.Enabled) return services;

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(t => t.FullName!.Replace("+", "."));
            });

            return services;
        }
        public static IServiceCollection AddMediatRConfig(this IServiceCollection services, IConfiguration config)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyMarker).Assembly);

                // If you already added ValidationBehavior, keep it first
                cfg.AddOpenBehavior(typeof(Application.Behaviors.ValidationBehavior<,>));

                // Add transaction behavior after validation
                cfg.AddOpenBehavior(typeof(Application.Behaviors.TransactionBehavior<,>));
            });


            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyMarker).Assembly);
                cfg.AddOpenBehavior(typeof(Application.Behaviors.ValidationBehavior<,>));
            });
            return services;
        }
    }
}
