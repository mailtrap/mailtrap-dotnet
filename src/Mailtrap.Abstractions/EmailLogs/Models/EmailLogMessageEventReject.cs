namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = reject.
/// </summary>
public sealed record EmailLogMessageEventReject : EmailLogMessageEvent
{
    /// <summary>
    /// Value of <c>event_type</c> for this event.
    /// </summary>
    public const string Discriminator = "reject";

    /// <inheritdoc />
    [JsonPropertyName("event_type")]
    [JsonPropertyOrder(-1)]
    public override string EventType => Discriminator;

    /// <summary>
    /// Reject-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsReject? Details { get; set; }
}
