namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Event details for event_type = soft_bounce or bounce.
/// </summary>
public sealed record EventDetailsBounce
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

    /// <summary>
    /// Email service provider status code.
    /// </summary>
    [JsonPropertyName("email_service_provider_status")]
    public string? EmailServiceProviderStatus { get; set; }

    /// <summary>
    /// Email service provider response message.
    /// </summary>
    [JsonPropertyName("email_service_provider_response")]
    public string? EmailServiceProviderResponse { get; set; }

    /// <summary>
    /// Bounce category.
    /// </summary>
    [JsonPropertyName("bounce_category")]
    public string? BounceCategory { get; set; }
}
