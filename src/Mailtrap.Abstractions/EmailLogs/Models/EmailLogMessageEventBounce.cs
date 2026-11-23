namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = bounce.
/// </summary>
public sealed record EmailLogMessageEventBounce : EmailLogMessageEvent
{
    /// <inheritdoc />
    public override string EventType => "bounce";

    /// <summary>
    /// Bounce-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsBounce? Details { get; set; }
}
