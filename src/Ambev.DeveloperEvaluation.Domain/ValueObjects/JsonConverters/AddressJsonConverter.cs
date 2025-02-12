using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects.JsonConverters;

public class AddressJsonConverter : JsonConverter<Address>
{
    public override Address? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        string? city = null;
        string? street = null;
        int number = 0;
        string? zipCode = null;
        string? latitude = null;
        string? longitude = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            string propertyName = reader.GetString()!;
            reader.Read();

            switch (propertyName.ToLower())
            {
                case "city":
                    city = reader.GetString();
                    break;
                case "street":
                    street = reader.GetString();
                    break;
                case "number":
                    number = reader.GetInt32();
                    break;
                case "zipcode":
                    zipCode = reader.GetString();
                    break;
                case "latitude":
                case "lat":
                    latitude = reader.GetString();
                    break;
                case "longitude":
                case "long":
                    longitude = reader.GetString();
                    break;
            }
        }

        return new Address(
            city ?? string.Empty,
            street ?? string.Empty,
            number,
            zipCode ?? string.Empty,
            latitude ?? string.Empty,
            longitude ?? string.Empty
        );
    }

    public override void Write(Utf8JsonWriter writer, Address? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString("city", value.City);
        writer.WriteString("street", value.Street);
        writer.WriteNumber("number", value.Number);
        writer.WriteString("zipCode", value.ZipCode);
        writer.WriteString("latitude", value.Latitude);
        writer.WriteString("longitude", value.Longitude);
        writer.WriteEndObject();
    }
}