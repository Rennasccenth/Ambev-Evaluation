using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects.JsonConverters;

public class RatingJsonConverter : JsonConverter<Rating>
{
    public override Rating Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string expectedRateName = options.PropertyNamingPolicy?.ConvertName(nameof(Rating.Rate)) ?? "rate";
        string expectedCountName = options.PropertyNamingPolicy?.ConvertName(nameof(Rating.Count)) ?? "count";

        decimal rate = 0;
        int count = 0;
        
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;
            
            if (reader.TokenType != JsonTokenType.PropertyName)
                continue;
            
            var propertyName = reader.GetString()!;
            reader.Read(); // go look for the value token
            
            if (propertyName == expectedRateName)
            {
                rate = reader.GetDecimal();
            }
            else if (propertyName == expectedCountName)
            {
                count = reader.GetInt32();
            }
        }
        
        return new Rating(rate, count);
    }

    public override void Write(Utf8JsonWriter writer, Rating value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        
        string rateProperty = options.PropertyNamingPolicy?.ConvertName(nameof(Rating.Rate)) ?? "rate";
        string countProperty = options.PropertyNamingPolicy?.ConvertName(nameof(Rating.Count)) ?? "count";
        
        writer.WriteNumber(rateProperty, value.Rate);
        writer.WriteNumber(countProperty, value.Count);
        writer.WriteEndObject();
    }
}