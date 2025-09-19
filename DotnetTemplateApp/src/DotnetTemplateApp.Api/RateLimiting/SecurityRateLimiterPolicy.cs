using System.Threading.RateLimiting;
using DotnetTemplateApp.Shared.ConfigurationSettings.Common;
using DotnetTemplateApp.Shared.ConfigurationSettings.RateLimiting;
using DotnetTemplateApp.Shared.Extensions;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

namespace DotnetTemplateApp.Api.RateLimiting
{
    public class SecurityRateLimiterPolicy(IOptions<RateLimitingSettings> settings, ILogger<SecurityRateLimiterPolicy> logger) : IRateLimiterPolicy<string>
    {
        public const string PolicyName = "Security";
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
            // partition consists of endpoint + user combination
            var userPartitionPart = string.Empty;
            var userAccountId = httpContext.Items["UserAccountId"];
            if (userAccountId != null)
            {
                userPartitionPart = userAccountId!.ToString()!;
            }
            else
            {
                if (httpContext.Request.Headers.TryGetValue(RequestHeaderConstants.DeviceIdHeader, out var deviceId))
                {
                    userPartitionPart = deviceId.ToString();
                }
            }

            var partitionKey = FormatPartitionKey(userPartitionPart, httpContext.Request.Path);

            return RateLimitPartition.GetFixedWindowLimiter(partitionKey,
                    partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = _settings.SecurityWindowRateLimit,
                        Window = TimeSpan.FromSeconds(_settings.SecurityRateLimitWindowInSeconds),
                        QueueLimit = _settings.SecurityWindowQueueLimit
                    });
        }

        private static string FormatPartitionKey(string userPartitionPart, string pathPartitionPart) =>
            $"{userPartitionPart}-{pathPartitionPart}";
    }
}
