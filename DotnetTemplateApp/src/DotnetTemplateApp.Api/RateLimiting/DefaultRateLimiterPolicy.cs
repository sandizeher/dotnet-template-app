using System.Threading.RateLimiting;
using DotnetTemplateApp.Shared.ConfigurationSettings.Common;
using DotnetTemplateApp.Shared.ConfigurationSettings.RateLimiting;
using DotnetTemplateApp.Shared.Extensions;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

namespace DotnetTemplateApp.Api.RateLimiting
{
    public class DefaultRateLimiterPolicy(IOptions<RateLimitingSettings> settings, ILogger<DefaultRateLimiterPolicy> logger) : IRateLimiterPolicy<string>
    {
        public const string PolicyName = "Default";
        private readonly RateLimitingSettings _settings = settings.GetSettings();

        public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected => async (context, _) =>
        {
            var userAccountId = ConvertHelpers.TryParseNullable<Guid>(context.HttpContext.Items["UserAccountId"]?.ToString());

            context.HttpContext.Request.Headers.TryGetValue(RequestHeaderConstants.DeviceIdHeader, out var deviceHeader);
            context.HttpContext.Request.Headers.TryGetValue(RequestHeaderConstants.IpAddressHeader, out var ipHeader);

            logger.LogWarning(
                "Request security rate limit reached for user account Id ({UserAccountId}), Device ID ({DeviceId}) and IP ({IpAddress}) on path '{Path}'",
                userAccountId, deviceHeader, ipHeader, context.HttpContext.Request.Path.ToString());

            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            {
                await context.HttpContext.Response.WriteAsync(
                    $"Too many requests. Please try again after {retryAfter.TotalSeconds} seconds.", _);
            }
            else
            {
                await context.HttpContext.Response.WriteAsync(
                    "Too many requests. Please try again later.", _);
            }
        };

        public RateLimitPartition<string> GetPartition(HttpContext httpContext)
        {
            // Policy for authenticated users partitioned by their names
            var userAccountId = httpContext.Items["UserAccountId"];
            if (userAccountId != null)
            {
                return RateLimitPartition.GetFixedWindowLimiter(userAccountId!.ToString()!,
                    partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = _settings.DefaultWindowRateLimit,
                        Window = TimeSpan.FromSeconds(_settings.DefaultRateLimitWindowInSeconds)
                    });
            }

            var partitionKey = httpContext.Request.Path.ToString();
            if (httpContext.Request.Headers.TryGetValue(RequestHeaderConstants.DeviceIdHeader, out var deviceHeader))
            {
                partitionKey += ':' + deviceHeader.ToString();
            }

            // Policy for unauthenticated users partitioned by the path
            return RateLimitPartition.GetFixedWindowLimiter(partitionKey,
                    partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = _settings.DefaultWindowRateLimit,
                        Window = TimeSpan.FromSeconds(_settings.DefaultRateLimitWindowInSeconds)
                    });
        }
    }
}
