using System.ComponentModel;
using System.Text.Json;

namespace DotnetTemplateApp.Shared.ConfigurationSettings.Common
{
    public static class ConvertHelpers
    {
        public static T ConvertStringToEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T? TryParseNullable<T>(this string? value)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null && value != null)
                {
                    return (T?)converter.ConvertFromString(value);
                }
                return default;
            }
            catch (NotSupportedException)
            {
                return default;
            }
        }

        public static DateTime? TryParseDateTimeAsUtcNullable(this string? value)
        {
            if (value != null && DateTime.TryParse(value, out var outValue))
            {
                return DateTime.SpecifyKind(outValue, DateTimeKind.Utc);
            }
            return null;
        }

        public static bool StringIfBase64(string value)
        {
            try
            {
                byte[]? output = Convert.FromBase64String(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ObjectToStringDictionary(object entity)
        {
            var dictionary = new Dictionary<string, object>();
            var type = entity.GetType();
            foreach (var property in type.GetProperties())
            {
                var value = property.GetValue(entity, null);
                if (value != null)
                {
                    dictionary.Add(property.Name, value);
                }
            }

            return JsonSerializer.Serialize(dictionary);
        }

        public static Dictionary<string, object> ObjectToDictionary(object entity)
        {
            var dictionary = new Dictionary<string, object>();
            var type = entity.GetType();
            foreach (var property in type.GetProperties())
            {
                var value = property.GetValue(entity, null);
                if (value != null)
                {
                    dictionary.Add(property.Name, value);
                }
            }

            return dictionary;
        }
    }
}
