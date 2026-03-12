namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Event details for event_type = click.
/// </summary>
public sealed record EventDetailsClick
{
    /// <summary>
    /// URL that was clicked.
    /// </summary>
    [JsonPropertyName("click_url")]
    public string? ClickUrl { get; set; }

    /// <summary>
    /// Web IP address that clicked.
    /// </summary>
    [JsonPropertyName("web_ip_address")]
    public string? WebIpAddress { get; set; }
}
