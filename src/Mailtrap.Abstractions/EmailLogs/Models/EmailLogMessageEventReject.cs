namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = reject.
/// </summary>
public sealed record EmailLogMessageEventReject : EmailLogMessageEvent
{
    /// <inheritdoc />
    public override string EventType => "reject";

    /// <summary>
    /// Reject-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsReject? Details { get; set; }
}
