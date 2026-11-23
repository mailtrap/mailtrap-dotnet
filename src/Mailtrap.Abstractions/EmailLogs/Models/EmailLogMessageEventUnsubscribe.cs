namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = unsubscribe.
/// </summary>
public sealed record EmailLogMessageEventUnsubscribe : EmailLogMessageEvent
{
    /// <summary>
    /// Value of <c>event_type</c> for this event.
    /// </summary>
    public const string Discriminator = "unsubscribe";

    /// <inheritdoc />
    [JsonPropertyName("event_type")]
    [JsonPropertyOrder(-1)]
    public override string EventType => Discriminator;

    /// <summary>
    /// Unsubscribe-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsUnsubscribe? Details { get; set; }
}
