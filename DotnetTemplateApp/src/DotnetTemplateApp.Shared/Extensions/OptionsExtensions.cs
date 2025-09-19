using Microsoft.Extensions.Options;

namespace DotnetTemplateApp.Shared.Extensions
{
    public static class OptionsExtensions
    {
        public static T GetSettings<T>(this IOptions<T> options) where T : class
        {
            return options.Value ?? throw new ArgumentNullException(nameof(options));
        }
    }
}
