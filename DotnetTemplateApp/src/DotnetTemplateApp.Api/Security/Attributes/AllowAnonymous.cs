using System.Diagnostics.CodeAnalysis;

namespace DotnetTemplateApp.Api.Security.Attributes
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute(bool full = false, bool requireDeviceIdHeader = false) : Attribute
    {
        /// <summary>
        /// WARNING: This will allow all requests to this endpoint to be anonymous, without doing the API key check!
        /// </summary>
        public bool Full { get; set; } = full;
        /// <summary>
        /// If true, the endpoint will require the DeviceId header to be present in the request
        /// </summary>
        public bool RequireDeviceIdHeader { get; set; } = requireDeviceIdHeader;
    }
}
