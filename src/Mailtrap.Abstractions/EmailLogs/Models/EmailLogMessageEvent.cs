namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Base type for an email log message event. Use <see cref="EventType"/> or pattern matching on the derived type to determine which details apply.
/// The discriminator in JSON is "event_type", which each derived type serializes first from <see cref="EventType"/>.
/// An event type this SDK version doesn't know is read as <see cref="EmailLogMessageEventUnknown"/>.
/// </summary>
[JsonConverter(typeof(EmailLogMessageEventJsonConverter))]
public abstract record EmailLogMessageEvent
{
    /// <summary>
    /// Event type identifier (matches the API event_type value). Derived types supply the value; it is never read from JSON.
    /// </summary>
    /// <remarks>
    /// Ignored here because each override maps it to <c>event_type</c>: attributes on an abstract property don't apply to overrides.
    /// </remarks>
    [JsonIgnore]
    public abstract string EventType { get; }

    /// <summary>
    /// When the event occurred.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }
}
