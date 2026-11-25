namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = click.
/// </summary>
public sealed record EmailLogMessageEventClick : EmailLogMessageEvent
{
    /// <summary>
    /// Value of <c>event_type</c> for this event.
    /// </summary>
    public const string Discriminator = "click";

    /// <inheritdoc />
    [JsonPropertyName("event_type")]
    [JsonPropertyOrder(-1)]
    public override string EventType => Discriminator;

    /// <summary>
    /// Click-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsClick? Details { get; set; }
}
