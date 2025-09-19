using System.Globalization;

namespace DotnetTemplateApp.Shared.ConfigurationSettings.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException() { }
        public InternalServerErrorException(string message) : base(message) { }
        public InternalServerErrorException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
