namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Event details for event_type = unsubscribe.
/// </summary>
public sealed record EventDetailsUnsubscribe
{
    /// <summary>
    /// Web IP address that unsubscribed.
    /// </summary>
    [JsonPropertyName("web_ip_address")]
    public string? WebIpAddress { get; set; }
}
