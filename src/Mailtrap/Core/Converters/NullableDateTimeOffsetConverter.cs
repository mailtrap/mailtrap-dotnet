using System.Runtime.CompilerServices;

namespace Mailtrap.Core.Converters;

/// <summary>
/// JSON converter for nullable <see cref="DateTimeOffset"/>.
/// It correctly handles empty strings as null values.
/// </summary>
public sealed class NullableDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
{
    // Cache a modified options instance per original options instance to avoid allocating on every call.
    private static readonly ConditionalWeakTable<JsonSerializerOptions, JsonSerializerOptions> s_optionsCache = new();

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

            if (DateTimeOffset.TryParse(stringValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue))
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
        // Remove this converter from options to avoid recursion.
        var optionsWithoutThisConverter = s_optionsCache.GetValue(options, key =>
        {
            // Create a single copy for this options instance and remove any instances of this converter type.
            var copy = new JsonSerializerOptions(key);

            for (var i = copy.Converters.Count - 1; i >= 0; i--)
            {
                if (copy.Converters[i] is NullableDateTimeOffsetConverter)
                {
                    copy.Converters.RemoveAt(i);
                }
            }

            return copy;
        });

        return JsonSerializer.Deserialize<DateTimeOffset?>(ref reader, optionsWithoutThisConverter);
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
