namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Event details for event_type = spam.
/// </summary>
public sealed record EventDetailsSpam
{
    /// <summary>
    /// Spam feedback type.
    /// </summary>
    [JsonPropertyName("spam_feedback_type")]
    public string? SpamFeedbackType { get; set; }
}
