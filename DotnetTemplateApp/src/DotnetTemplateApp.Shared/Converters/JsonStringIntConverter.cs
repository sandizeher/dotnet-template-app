using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotnetTemplateApp.Shared.Converters
{
    public class JsonStringIntConverter : JsonConverter<int>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(int);
        }

        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32();
            }

            if (!int.TryParse(reader.GetString(), out var value))
            {
                return default;
            }

            return value;
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
