namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Event details for event_type = suspension or reject.
/// </summary>
public sealed record EventDetailsReject
{
    /// <summary>
    /// Reject reason.
    /// </summary>
    [JsonPropertyName("reject_reason")]
    public string? RejectReason { get; set; }
}
