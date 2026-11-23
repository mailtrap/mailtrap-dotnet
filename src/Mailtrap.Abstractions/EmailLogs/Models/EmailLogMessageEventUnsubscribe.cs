namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = unsubscribe.
/// </summary>
public sealed record EmailLogMessageEventUnsubscribe : EmailLogMessageEvent
{
    /// <inheritdoc />
    public override string EventType => "unsubscribe";

    /// <summary>
    /// Unsubscribe-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsUnsubscribe? Details { get; set; }
}
