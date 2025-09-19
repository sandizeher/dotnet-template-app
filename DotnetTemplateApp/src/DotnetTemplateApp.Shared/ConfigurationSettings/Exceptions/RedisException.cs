using System.Globalization;

namespace DotnetTemplateApp.Shared.ConfigurationSettings.Exceptions
{
    public class RedisException : Exception
    {
        public RedisException() { }
        public RedisException(string message) : base(message) { }
        public RedisException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
