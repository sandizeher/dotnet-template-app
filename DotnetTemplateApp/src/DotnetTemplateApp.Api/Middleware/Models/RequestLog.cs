using System.Diagnostics.CodeAnalysis;

namespace DotnetTemplateApp.Api.Middleware.Models
{
    [ExcludeFromCodeCoverage]
    public class RequestLog
    {
        public string? Headers { get; set; }
        public string? Token { get; set; }
        public string? Body { get; set; }
        public string? RequestUrl { get; set; }
        public string? ContentType { get; set; }
        public string? IpAddress { get; set; }
        public string? AppVersion { get; set; }
    }
}
