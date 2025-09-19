using System.Diagnostics.CodeAnalysis;

namespace DotnetTemplateApp.Shared.ConfigurationSettings.RateLimiting
{
    [ExcludeFromCodeCoverage]
    public class RateLimitingSettings
    {
        /// <summary>
        /// Fixed window size for the <see cref="DefaultRateLimiterPolicy"/> in seconds (default 5 seconds)
        /// </summary>
        public int DefaultRateLimitWindowInSeconds { get; set; } = 5;
        /// <summary>
        /// Rate limit for the fixed window size in <see cref="DefaultRateLimiterPolicy"/> (default 100 requests)
        /// </summary>
        public int DefaultWindowRateLimit { get; set; } = 100;
        /// <summary>
        /// Fixed window size for the <see cref="SecurityRateLimiterPolicy"/> in seconds (default 4 seconds)
        /// </summary>
        public int SecurityRateLimitWindowInSeconds { get; set; } = 4;
        /// <summary>
        /// Rate limit for the fixed window size in <see cref="SecurityRateLimiterPolicy"/> (default 5 requests)
        /// </summary>
        public int SecurityWindowRateLimit { get; set; } = 5;
        /// <summary>
        /// Window queue size for the <see cref="SecurityRateLimiterPolicy"/> (default 5 requests)
        /// </summary>
        public int SecurityWindowQueueLimit { get; set; } = 5;
    }
}
