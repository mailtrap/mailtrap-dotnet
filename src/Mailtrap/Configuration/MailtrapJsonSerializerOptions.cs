namespace Mailtrap.Configuration;


internal static class MailtrapJsonSerializerOptions
{
    internal static JsonSerializerOptions Default { get; } = new JsonSerializerOptions(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters =
        {
            new StringEnumJsonConverterFactory(),
            new NullableDateTimeOffsetConverter()
        }
    };

    internal static JsonSerializerOptions NotIndented { get; } = new JsonSerializerOptions(Default)
    {
        WriteIndented = false
    };
}
