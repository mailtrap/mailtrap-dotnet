namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = soft_bounce.
/// </summary>
public sealed record EmailLogMessageEventSoftBounce : EmailLogMessageEvent
{
    /// <summary>
    /// Value of <c>event_type</c> for this event.
    /// </summary>
    public const string Discriminator = "soft_bounce";

    /// <inheritdoc />
    [JsonPropertyName("event_type")]
    [JsonPropertyOrder(-1)]
    public override string EventType => Discriminator;

    /// <summary>
    /// Bounce-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsBounce? Details { get; set; }
}
