namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Base type for an email log message event. Use <see cref="EventType"/> or pattern matching on the derived type to determine which details apply.
/// The polymorphic discriminator in JSON is "event_type"; this type does not declare a matching property so the converter can use it exclusively.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "event_type")]
[JsonDerivedType(typeof(EmailLogMessageEventDelivery), "delivery")]
[JsonDerivedType(typeof(EmailLogMessageEventOpen), "open")]
[JsonDerivedType(typeof(EmailLogMessageEventClick), "click")]
[JsonDerivedType(typeof(EmailLogMessageEventSoftBounce), "soft_bounce")]
[JsonDerivedType(typeof(EmailLogMessageEventBounce), "bounce")]
[JsonDerivedType(typeof(EmailLogMessageEventSpam), "spam")]
[JsonDerivedType(typeof(EmailLogMessageEventUnsubscribe), "unsubscribe")]
[JsonDerivedType(typeof(EmailLogMessageEventSuspension), "suspension")]
[JsonDerivedType(typeof(EmailLogMessageEventReject), "reject")]
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
