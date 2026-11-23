namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = open.
/// </summary>
public sealed record EmailLogMessageEventOpen : EmailLogMessageEvent
{
    /// <summary>
    /// Value of <c>event_type</c> for this event.
    /// </summary>
    public const string Discriminator = "open";

    /// <inheritdoc />
    [JsonPropertyName("event_type")]
    [JsonPropertyOrder(-1)]
    public override string EventType => Discriminator;

    /// <summary>
    /// Open-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsOpen? Details { get; set; }
}
