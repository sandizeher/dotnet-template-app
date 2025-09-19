namespace DotnetTemplateApp.Shared.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime() <= DateTimeOffset.MinValue.UtcDateTime
                       ? DateTimeOffset.MinValue
                       : new DateTimeOffset(dateTime);
        }

        public static DateTime UnixTimeToDateTime(long unixTime)
        {
            var dateTime = DateTime.UnixEpoch;
            return dateTime.AddSeconds(unixTime);
        }
    }
}
