namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event of an event_type this SDK version doesn't know.
/// Keeps the raw <see cref="EventType"/> and <see cref="Details"/>, so reading a message never fails on a new event type.
/// </summary>
public sealed record EmailLogMessageEventUnknown : EmailLogMessageEvent
{
    private readonly string _eventType;


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


    /// <inheritdoc />
    public override string EventType => _eventType;

    /// <summary>
    /// Raw event details, or <see langword="null"/> when the event has none.
    /// </summary>
    [JsonPropertyName("details")]
    public JsonElement? Details { get; set; }
}
