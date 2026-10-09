namespace Mailtrap.Inbound.Models;


/// <summary>
/// Represents an inbound forward rule destination.
/// </summary>
public sealed record InboundForwardRuleDestination
{
    /// <summary>
    /// Gets or sets the destination email address.
    /// </summary>
    ///
    /// <value>
    /// Destination email address.
    /// </value>
    [JsonPropertyName("email")]
    public string? Email { get; set; }
}
