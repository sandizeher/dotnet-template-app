using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotnetTemplateApp.Shared.Converters
{
    public class JsonStringLongConverter : JsonConverter<long>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(long);
        }

        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                reader.TryGetInt64(out var value);
                return value;
            }
            catch
            {
                // If the value is not long type, try to parse it as a string
                long.TryParse(reader.GetString(), out var value);
                return value;
            }
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
