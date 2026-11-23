namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = suspension.
/// </summary>
public sealed record EmailLogMessageEventSuspension : EmailLogMessageEvent
{
    /// <inheritdoc />
    public override string EventType => "suspension";

    /// <summary>
    /// Reject-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsReject? Details { get; set; }
}
