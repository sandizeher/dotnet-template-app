using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotnetTemplateApp.Shared.Converters
{
    public class JsonStringDateTimeConverter : JsonConverter<DateTimeOffset>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(DateTimeOffset);
        }

        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            DateTimeOffset.TryParse(reader.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var value);
            return value;
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
