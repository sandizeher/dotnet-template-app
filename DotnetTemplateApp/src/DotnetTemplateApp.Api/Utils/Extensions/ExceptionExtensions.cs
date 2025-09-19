using System.Diagnostics.CodeAnalysis;
using DotnetTemplateApp.Api.Middleware;

namespace DotnetTemplateApp.Api.Utils.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ExceptionExtensions
    {
        public static IApplicationBuilder UseAppExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            return app;
        }

        public static IApplicationBuilder UseAppExceptionHandler(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            return app;
        }
    }
}
