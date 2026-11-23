namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = spam.
/// </summary>
public sealed record EmailLogMessageEventSpam : EmailLogMessageEvent
{
    /// <summary>
    /// Value of <c>event_type</c> for this event.
    /// </summary>
    public const string Discriminator = "spam";

    /// <inheritdoc />
    [JsonPropertyName("event_type")]
    [JsonPropertyOrder(-1)]
    public override string EventType => Discriminator;

    /// <summary>
    /// Spam-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsSpam? Details { get; set; }
}
