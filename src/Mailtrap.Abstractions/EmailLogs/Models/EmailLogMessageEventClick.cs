namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = click.
/// </summary>
public sealed record EmailLogMessageEventClick : EmailLogMessageEvent
{
    /// <inheritdoc />
    [JsonPropertyName("event_type")]
    [JsonPropertyOrder(-1)]
    public override string EventType => "click";

    /// <summary>
    /// Click-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsClick? Details { get; set; }
}
