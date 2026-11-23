namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = spam.
/// </summary>
public sealed record EmailLogMessageEventSpam : EmailLogMessageEvent
{
    /// <inheritdoc />
    public override string EventType => "spam";

    /// <summary>
    /// Spam-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsSpam? Details { get; set; }
}
