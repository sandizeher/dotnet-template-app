using DotnetTemplateApp.Shared.ConfigurationSettings.DbSettings;
using DotnetTemplateApp.Shared.ConfigurationSettings.RateLimiting;
using DotnetTemplateApp.Shared.ConfigurationSettings.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace DotnetTemplateApp.Shared
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddShared(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfigurationSettings(configuration);
            return services;
        }
        private static void AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(opt => configuration.Bind(nameof(JwtOptions), opt));
            services.Configure<DbSettings>(opt => configuration.Bind(nameof(DbSettings), opt));
            services.Configure<AccessOptions>(opt => configuration.Bind(nameof(AccessOptions), opt));
            services.Configure<RateLimitingSettings>(opt => configuration.Bind(nameof(RateLimitingSettings), opt));
        }
    }
}
