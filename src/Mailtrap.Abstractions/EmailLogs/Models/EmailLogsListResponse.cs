namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Response for listing email logs.
/// </summary>
public sealed record EmailLogsListResponse
{
    /// <summary>
    /// Page of messages.
    /// </summary>
    [JsonPropertyName("messages")]
    public IList<EmailLogMessage> Messages { get; set; } = [];

    /// <summary>
    /// Total number of messages matching the filters (before pagination).
    /// </summary>
    [JsonPropertyName("total_count")]
    public int TotalCount { get; set; }

    /// <summary>
    /// Message UUID to use as search_after for the next page, or null if no more pages.
    /// </summary>
    [JsonPropertyName("next_page_cursor")]
    public string? NextPageCursor { get; set; }
}
