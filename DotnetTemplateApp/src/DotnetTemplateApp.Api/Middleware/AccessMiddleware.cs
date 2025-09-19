using DotnetTemplateApp.Shared.ConfigurationSettings.Common;
using DotnetTemplateApp.Shared.ConfigurationSettings.Security;
using Microsoft.Extensions.Options;

namespace DotnetTemplateApp.Api.Middleware
{
    public class AccessMiddleware(
        RequestDelegate next,
        IOptions<AccessOptions> options)
    {
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/favicon.ico")) return;

            var xApiKey = context.Request.Headers[RequestHeaderConstants.ApiKeyHeader].FirstOrDefault();
            if (xApiKey != null && options.Value.ApiKey == xApiKey)
            {
                context.Items["XApiKey"] = xApiKey;
                context.Items["DeviceId"] = context.Request.Headers[RequestHeaderConstants.DeviceIdHeader].FirstOrDefault();
                context.Items[RequestHeaderConstants.IpAddressHeader] = context.Request.Headers[RequestHeaderConstants.IpAddressHeader].FirstOrDefault();
            }

            await next(context);
        }
    }
}
