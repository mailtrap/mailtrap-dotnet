namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Event details for event_type = delivery.
/// </summary>
public sealed record EventDetailsDelivery
{
    /// <summary>
    /// Sending IP address.
    /// </summary>
    [JsonPropertyName("sending_ip")]
    public string? SendingIp { get; set; }

    /// <summary>
    /// Recipient MX host.
    /// </summary>
    [JsonPropertyName("recipient_mx")]
    public string? RecipientMx { get; set; }

    /// <summary>
    /// Email service provider name.
    /// </summary>
    [JsonPropertyName("email_service_provider")]
    public string? EmailServiceProvider { get; set; }
}
