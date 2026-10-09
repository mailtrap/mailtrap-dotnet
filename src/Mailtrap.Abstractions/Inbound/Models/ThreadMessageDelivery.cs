namespace Mailtrap.Inbound.Models;


/// <summary>
/// Represents the delivery outcome of a sent thread message.
/// </summary>
public sealed record ThreadMessageDelivery
{
    /// <summary>
    /// Gets or sets the recipient address.
    /// </summary>
    ///
    /// <value>
    /// Recipient address.
    /// </value>
    [JsonPropertyName("to")]
    public string? To { get; set; }

    /// <summary>
    /// Gets or sets the delivery status.
    /// </summary>
    ///
    /// <value>
    /// Delivery status.
    /// </value>
    [JsonPropertyName("status")]
    public EmailLogStatus Status { get; set; } = EmailLogStatus.Unknown;

    /// <summary>
    /// Gets or sets the timestamp when the message was delivered.
    /// </summary>
    ///
    /// <value>
    /// Timestamp when the message was delivered.
    /// </value>
    [JsonPropertyName("delivered_at")]
    public DateTimeOffset? DeliveredAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the message hard-bounced.
    /// </summary>
    ///
    /// <value>
    /// Timestamp when the message hard-bounced.
    /// </value>
    [JsonPropertyName("bounced_at")]
    public DateTimeOffset? BouncedAt { get; set; }
}
