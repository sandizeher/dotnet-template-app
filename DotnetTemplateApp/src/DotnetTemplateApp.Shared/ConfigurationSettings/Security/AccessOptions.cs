using System.Diagnostics.CodeAnalysis;

namespace DotnetTemplateApp.Shared.ConfigurationSettings.Security
{
    [ExcludeFromCodeCoverage]
    public class AccessOptions
    {
        public string? ApiKey { get; set; }
        public string? WebAccessApiKey { get; set; }
    }
}
