namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Event details for event_type = open.
/// </summary>
public sealed record EventDetailsOpen
{
    /// <summary>
    /// Web IP address that opened the message.
    /// </summary>
    [JsonPropertyName("web_ip_address")]
    public string? WebIpAddress { get; set; }
}
