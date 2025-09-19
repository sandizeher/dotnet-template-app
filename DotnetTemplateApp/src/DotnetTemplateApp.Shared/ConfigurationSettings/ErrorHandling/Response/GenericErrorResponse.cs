namespace DotnetTemplateApp.Shared.ConfigurationSettings.ErrorHandling.Response
{
    public static class GenericErrorResponse
    {
        public static ErrorDetails ErrorOccured => new(ErrorType.ErrorOccured, message: "Error occured.");
        public static ErrorDetails UnauthorizedAccess => new(ErrorType.UnauthorizedAccess, message: "Unauthorized access.");
        public static ErrorDetails NotFound => new(ErrorType.NotFound, message: "Not found.");
        public static ErrorDetails UnknownJsonData => new(ErrorType.UnknownJsonData, message: "Unknown JSON data.");
        public static ErrorDetails AccessTokenExpired => new(ErrorType.AccessTokenExpired, message: "Access token expired.");
        public static ErrorDetails SomethingWentWrong => new(
            ErrorType.ErrorOccured,
            "Something went wrong",
            "Please try again later");
    }
}
