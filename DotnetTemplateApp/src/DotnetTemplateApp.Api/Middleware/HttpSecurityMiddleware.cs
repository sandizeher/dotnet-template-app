namespace DotnetTemplateApp.Api.Middleware
{
    public class HttpSecurityMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("Referrer-Policy", "origin-when-cross-origin");
            context.Response.Headers.Append("X-Frame-Options", "SAMEORIGIN");
            context.Response.Headers.Append("X-Xss-Protection", "1; mode=block");
            context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'");

            await next(context);
        }
    }
}
