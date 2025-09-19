using System.Globalization;

namespace DotnetTemplateApp.Shared.ConfigurationSettings.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() : base() { }

        public ForbiddenAccessException(string message) : base(message) { }

        public ForbiddenAccessException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
