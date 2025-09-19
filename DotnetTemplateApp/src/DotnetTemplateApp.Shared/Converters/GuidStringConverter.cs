using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotnetTemplateApp.Shared.Converters
{
    public class GuidStringConverter : JsonConverter<Guid>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(Guid);
        }

        public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Guid.TryParse(reader.GetString(), out Guid value);
            return value;
        }

        public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
