using DotnetTemplateApp.Api.Security.Attributes;
using DotnetTemplateApp.Api.Security.Jwt.Interfaces;
using DotnetTemplateApp.Shared.ConfigurationSettings.ErrorHandling.Response;
using DotnetTemplateApp.Shared.Extensions;
using System.Text.Json;

namespace DotnetTemplateApp.Api.Middleware
{
    public class JwtMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context, IJwtUtils jwtUtils)
        {
            var anonymousAccessAllowed = (context.GetEndpoint()?.Metadata
                .Any(m => m is AllowAnonymousAttribute))
                .GetValueOrDefault();

            if (anonymousAccessAllowed)
            {
                await next(context);
                return;
            }

            var token = await HandleToken(context);

            if (token.IsNullOrEmpty()) return;

            var jwtTokenClaims = await jwtUtils.ValidateJwtToken(token)
                ?? throw new UnauthorizedAccessException();

            context.Items["UserAccountId"] = jwtTokenClaims.UserAccountId;
            context.Items["UserId"] = jwtTokenClaims.UserId;

            await next(context);
        }

        private static async Task<string?> HandleToken(HttpContext context)
        {
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split()[^1];

            if (!token.IsNullOrEmpty()) return token!;

            // Handle the empty token scenario without throwing an exception
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = GenericErrorResponse.UnauthorizedAccess.Message }));
            return null;
        }
    }
}
