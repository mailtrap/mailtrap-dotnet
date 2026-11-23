namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = unsubscribe.
/// </summary>
public sealed record EmailLogMessageEventUnsubscribe : EmailLogMessageEvent
{
    /// <inheritdoc />
    [JsonPropertyName("event_type")]
    [JsonPropertyOrder(-1)]
    public override string EventType => "unsubscribe";

    /// <summary>
    /// Unsubscribe-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsUnsubscribe? Details { get; set; }
}
