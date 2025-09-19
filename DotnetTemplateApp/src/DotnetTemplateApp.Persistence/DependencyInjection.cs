using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using DotnetTemplateApp.Domain.Interfaces.Persistence;
using DotnetTemplateApp.Domain.Interfaces.Persistence.Repositories.Account;
using DotnetTemplateApp.Persistence.Contexts;
using DotnetTemplateApp.Persistence.Repositories;
using DotnetTemplateApp.Persistence.Repositories.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetTemplateApp.Persistence
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataSources(configuration);
            services.AddUserRepositories();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        private static void AddUserRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserAccountRepository, UserAccountRepository>();
        }

        private static void AddDataSources(this IServiceCollection services, IConfiguration configuration)
        {
            // initialize the NpgSqlDataSource
            services.AddNpgsqlDataSource(
                configuration.GetSection("DbSettings:ConnectionString").Value!,
                builder =>
                {
                    builder.UseLoggerFactory(null);
                },
                ServiceLifetime.Transient,
                ServiceLifetime.Singleton,
                serviceKey: "NpgsqlDataSource");

            // use NpgSqlDataSource as the default data source for EF Core
            services.AddDbContext<ApplicationDbContext>((provider, options) =>
            {
                options.UseNpgsql(
                    provider.GetKeyedService<DbDataSource>("NpgsqlDataSource")!,
                    options =>
                    {
                        options.EnableRetryOnFailure();
                    });
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            },
            contextLifetime: ServiceLifetime.Scoped,
            optionsLifetime: ServiceLifetime.Singleton);
        }
    }
}
