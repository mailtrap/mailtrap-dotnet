namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = soft_bounce.
/// </summary>
public sealed record EmailLogMessageEventSoftBounce : EmailLogMessageEvent
{
    /// <inheritdoc />
    public override string EventType => "soft_bounce";

    /// <summary>
    /// Bounce-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsBounce? Details { get; set; }
}
