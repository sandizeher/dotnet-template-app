using DotnetTemplateApp.Api.Middleware.Models;
using DotnetTemplateApp.Shared.ConfigurationSettings.Common;
using DotnetTemplateApp.Shared.ConfigurationSettings.ErrorHandling.Response;
using DotnetTemplateApp.Shared.ConfigurationSettings.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace DotnetTemplateApp.Api.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (!context.Request.Body.CanSeek)
                {
                    context.Request.EnableBuffering();
                }

                await next(context);
            }
            catch (SecurityTokenExpiredException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = GenericErrorResponse.AccessTokenExpired.Message }));
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = GenericErrorResponse.UnauthorizedAccess.Message }));
                await LogError(context, ex);
            }
            catch (ForbiddenAccessException ex)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await LogError(context, ex);
            }
            catch (ApiException ex)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await LogError(context, ex);
            }
            catch (DbUpdateException dbex)
            {
                var innerException = dbex.InnerException ?? dbex;
                // EntityFrameworkCore.Exceptions.Common
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await LogError(context, innerException);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await LogError(context, ex);
            }
        }

        private async Task<RequestLog> RequestAdditionalInfo(HttpContext context)
        {
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split()[^1];
            context.Request.Headers.TryGetValue("X-Client-IP", out var ipHeader);

            var requestLog = new RequestLog
            {
                Headers = GetHeaders(context),
                Token = token,
                Body = await GetRequestBodyText(context.Request),
                RequestUrl = context.Request.Path.ToString(),
                ContentType = context.Request.ContentType,
                IpAddress = ipHeader.ToString(),
            };

            return requestLog;
        }

        private static string GetHeaders(HttpContext context)
        {
            var sb = new StringBuilder();
            foreach (var header in context.Request.Headers)
            {
                sb.Append($"{header.Key}:{header.Value}\n");
            }
            return sb.ToString();
        }

        private async Task<string> GetRequestBodyText(HttpRequest request)
        {
            try
            {
                if (request.Body.CanRead)
                {
                    request.Body.Position = 0;
                    using var reader = new StreamReader(request.Body, encoding: Encoding.Default);
                    var text = await reader.ReadToEndAsync();
                    return text;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while reading request body");
            }

            return string.Empty;
        }

        private async Task LogError(HttpContext context, Exception ex)
        {
            var requestLog = await RequestAdditionalInfo(context);
            var userAccountId = ConvertHelpers.TryParseNullable<Guid>(context.Items["UserAccountId"]?.ToString());
            var userId = ConvertHelpers.TryParseNullable<Guid>(context.Items["UserId"]?.ToString());

            logger.LogError(ex,
                "Request Details: headers {RequestHeaders}, Body {RequestBody}, Url {RequestUrl}, " +
                "ContentType {ContentType}, IpAddress {IpAddress}, Token {Token}, UserId {UserId}, UserAccountId {UserAccountId}, AppVersion {AppVersion}",
                requestLog.Headers,
                requestLog.Body,
                requestLog.RequestUrl,
                requestLog.ContentType,
                requestLog.IpAddress,
                requestLog.Token,
                userId,
                userAccountId,
                requestLog.AppVersion);
        }
    }
}
