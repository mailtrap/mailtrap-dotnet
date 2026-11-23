namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = open.
/// </summary>
public sealed record EmailLogMessageEventOpen : EmailLogMessageEvent
{
    /// <inheritdoc />
    [JsonPropertyName("event_type")]
    [JsonPropertyOrder(-1)]
    public override string EventType => "open";

    /// <summary>
    /// Open-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsOpen? Details { get; set; }
}
