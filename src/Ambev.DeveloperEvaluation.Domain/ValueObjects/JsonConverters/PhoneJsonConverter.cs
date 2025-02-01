using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects.JsonConverters;

public class PhoneJsonConverter : JsonConverter<Phone>
{
    public override Phone? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        return value == null ? null : (Phone)value;
    }

    public override void Write(Utf8JsonWriter writer, Phone? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString());
    }
}