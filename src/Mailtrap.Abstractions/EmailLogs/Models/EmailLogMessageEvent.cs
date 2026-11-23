namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Base type for an email log message event. Use <see cref="EventType"/> or pattern matching on the derived type to determine which details apply.
/// The discriminator in JSON is "event_type"; this type does not declare a matching property so the converter can use it exclusively.
/// An event type this SDK version doesn't know is read as <see cref="EmailLogMessageEventUnknown"/>.
/// </summary>
[JsonConverter(typeof(EmailLogMessageEventJsonConverter))]
public abstract record EmailLogMessageEvent
{
    /// <summary>
    /// Event type identifier (matches the API event_type value). Not serialized from JSON; derived types supply the value.
    /// </summary>
    [JsonIgnore]
    public abstract string EventType { get; }

    /// <summary>
    /// When the event occurred.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }
}
