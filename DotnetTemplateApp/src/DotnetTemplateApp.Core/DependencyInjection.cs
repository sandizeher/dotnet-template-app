using DotnetTemplateApp.Core.Services.Account;
using DotnetTemplateApp.Core.Services.Account.Interfaces;
using DotnetTemplateApp.Persistence;
using DotnetTemplateApp.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetTemplateApp.Core
{
    public static class DependencyInjection
    {
        public static async Task ExecuteDatabaseMigrations(this IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(5));

            await dbContext.Database.MigrateAsync();
        }

        public static IServiceCollection AddApplicationCore(this IServiceCollection services, IConfiguration configuration, string env)
        {
            services.AddAccountService();

            services.AddApplicationPersistence(configuration);

            return services;
        }

                private static void AddAccountService(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserAccountService, UserAccountService>();
        }
    }
}
