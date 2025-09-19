namespace DotnetTemplateApp.Shared.ConfigurationSettings.ErrorHandling
{
    public class ErrorDetails(ErrorType errorCode, string title = default!, string message = default!)
    {
        public ErrorType ErrorCode { get; set; } = errorCode;
        public string? Title { get; set; } = title;
        public string? Message { get; set; } = message;
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
    }
}
