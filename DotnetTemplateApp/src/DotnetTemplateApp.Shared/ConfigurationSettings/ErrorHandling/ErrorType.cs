namespace DotnetTemplateApp.Shared.ConfigurationSettings.ErrorHandling
{
    public enum ErrorType
    {
        None = 0,
        // General
        ValidationError = 400,
        UnderMaintenance = 503,
        ErrorOccured = 1001,
        UnauthorizedAccess = 1002,
        NotFound = 1003,
        UnknownJsonData = 1004,
        AccessTokenExpired = 1005,
        RequestStillProcessing = 1006,
        ActionNotAllowed = 1007,
        CertificateNotValid = 1008,
    }
}
