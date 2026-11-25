namespace Mailtrap.EmailLogs.Converters;


/// <summary>
/// Custom JSON converter to be used for <see cref="EmailLogMessageEvent"/>.<br/>
/// Reads the derived event type named by the <c>event_type</c> property, falling back to
/// <see cref="EmailLogMessageEventUnknown"/> for a type it doesn't know, and writes the derived type.
/// </summary>
///
/// <remarks>
/// Attribute polymorphism would throw on an unknown <c>event_type</c> and fail the whole message.<br/>
/// A type-level <see cref="JsonConverterAttribute"/> isn't inherited, so the derived types are read and
/// written by the default converters and this converter doesn't recurse.
/// </remarks>
internal sealed class EmailLogMessageEventJsonConverter : JsonConverter<EmailLogMessageEvent>
{
    private const string EventTypePropertyName = "event_type";

    private static readonly Dictionary<string, Type> s_eventTypes = new(StringComparer.Ordinal)
    {
        [EmailLogMessageEventDelivery.Discriminator] = typeof(EmailLogMessageEventDelivery),
        [EmailLogMessageEventOpen.Discriminator] = typeof(EmailLogMessageEventOpen),
        [EmailLogMessageEventClick.Discriminator] = typeof(EmailLogMessageEventClick),
        [EmailLogMessageEventSoftBounce.Discriminator] = typeof(EmailLogMessageEventSoftBounce),
        [EmailLogMessageEventBounce.Discriminator] = typeof(EmailLogMessageEventBounce),
        [EmailLogMessageEventSpam.Discriminator] = typeof(EmailLogMessageEventSpam),
        [EmailLogMessageEventUnsubscribe.Discriminator] = typeof(EmailLogMessageEventUnsubscribe),
        [EmailLogMessageEventSuspension.Discriminator] = typeof(EmailLogMessageEventSuspension),
        [EmailLogMessageEventReject.Discriminator] = typeof(EmailLogMessageEventReject),
        [EmailLogMessageEventTopicUnsubscribe.Discriminator] = typeof(EmailLogMessageEventTopicUnsubscribe),
        [EmailLogMessageEventTopicResubscribe.Discriminator] = typeof(EmailLogMessageEventTopicResubscribe)
    };


    public override EmailLogMessageEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        if (root.ValueKind != JsonValueKind.Object)
        {
            throw new JsonException($"Unexpected JSON value {root.ValueKind} for {nameof(EmailLogMessageEvent)}.");
        }

        var eventType = root.TryGetProperty(EventTypePropertyName, out var eventTypeElement)
            && eventTypeElement.ValueKind == JsonValueKind.String
                ? eventTypeElement.GetString()!
                : string.Empty;

        var type = s_eventTypes.TryGetValue(eventType, out var knownType)
            ? knownType
            : typeof(EmailLogMessageEventUnknown);

        return (EmailLogMessageEvent?)root.Deserialize(type, options);
    }

    // Each derived type writes its event_type first, from its EventType property.
    public override void Write(Utf8JsonWriter writer, EmailLogMessageEvent value, JsonSerializerOptions options)
    {
        Ensure.NotNull(value, nameof(value));

        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
