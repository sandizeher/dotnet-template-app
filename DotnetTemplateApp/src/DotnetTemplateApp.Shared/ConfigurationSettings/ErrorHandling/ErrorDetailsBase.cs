namespace DotnetTemplateApp.Shared.ConfigurationSettings.ErrorHandling
{
    public abstract record ErrorDetailsBase
    {
        public ErrorDetails? ErrorDetails { get; set; }
    }
}
