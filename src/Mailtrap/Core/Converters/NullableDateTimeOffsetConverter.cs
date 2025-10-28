namespace Mailtrap.Core.Converters;

/// <summary>
/// JSON converter for nullable <see cref="DateTimeOffset"/>.
/// It correctly handles empty strings as null values.
/// </summary>
public sealed class NullableDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
{
    /// <inheritdoc />
    /// <exception cref="JsonException"></exception>
    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return null;
            }

            if (DateTimeOffset.TryParse(stringValue, out var dateValue))
            {
                return dateValue;
            }

            throw new JsonException($"Cannot convert value '{stringValue}' to {nameof(DateTimeOffset)}");
        }

        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        // Fallback for non-string tokens (though unlikely for DateTimeOffset)
        return JsonSerializer.Deserialize<DateTimeOffset?>(ref reader, options);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        Ensure.NotNull(writer, nameof(writer));

        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
