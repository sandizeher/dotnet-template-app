using System.Diagnostics.CodeAnalysis;

namespace DotnetTemplateApp.Shared.ConfigurationSettings.DbSettings
{
    [ExcludeFromCodeCoverage]
    public class DbSettings
    {
        public string ConnectionString { get; set; } = default!;
    }
}
