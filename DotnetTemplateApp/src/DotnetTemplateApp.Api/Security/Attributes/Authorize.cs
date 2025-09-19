using DotnetTemplateApp.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;

namespace DotnetTemplateApp.Api.Security.Attributes
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute() : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var endpoint = context.HttpContext.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>();
            if (allowAnonymous != null && allowAnonymous.Full)
            {
                return;
            }

            // Check required device Id
            if (allowAnonymous != null && allowAnonymous.RequireDeviceIdHeader)
            {
                var deviceId = context.HttpContext.Items["DeviceId"]?.ToString();
                if (deviceId.IsNullOrWhiteSpace())
                {
                    context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                    return;
                }
            }

            // Regular anonymous checks api key validity only
            var xApiKey = context.HttpContext.Items["XApiKey"]?.ToString();
            if (allowAnonymous != null && xApiKey == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }
        }
    }
}
