namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event of an event_type this SDK version doesn't know.
/// Keeps the raw <see cref="EventType"/> and <see cref="Details"/>, so reading a message never fails on a new event type.
/// </summary>
public sealed record EmailLogMessageEventUnknown : EmailLogMessageEvent
{
    /// <summary>
    /// Creates an event of the given <paramref name="eventType"/>.
    /// </summary>
    ///
    /// <param name="eventType">
    /// Raw event type, as the API returned it.
    /// </param>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="eventType"/> is <see langword="null"/>.
    /// </exception>
    public EmailLogMessageEventUnknown(string eventType)
    {
        Ensure.NotNull(eventType, nameof(eventType));

        _eventType = eventType;
    }

    // For the serializer, which fills the event type through RawEventType when this type is read directly:
    // the public constructor's parameter would bind to both EventType declarations.
    [JsonConstructor]
    private EmailLogMessageEventUnknown()
    {
        _eventType = string.Empty;
    }


    private string _eventType;

    /// <inheritdoc />
    [JsonIgnore]
    public override string EventType => _eventType;

    // Maps event_type both ways: an override can't add the setter the serializer needs.
    [JsonInclude]
    [JsonPropertyName("event_type")]
    [JsonPropertyOrder(-1)]
    private string RawEventType
    {
        get => _eventType;
        set => _eventType = value ?? string.Empty;
    }

    /// <summary>
    /// Raw event details, or <see langword="null"/> when the event has none.
    /// </summary>
    [JsonPropertyName("details")]
    public JsonElement? Details { get; set; }
}
