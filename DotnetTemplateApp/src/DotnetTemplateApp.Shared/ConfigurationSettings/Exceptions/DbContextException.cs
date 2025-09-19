namespace DotnetTemplateApp.Shared.ConfigurationSettings.Exceptions
{
    public class DbContextException : Exception
    {
        public DbContextException() { }
        public DbContextException(string message) : base(message) { }
        public DbContextException(string message, Exception inner) : base(message, inner) { }
    }
}
