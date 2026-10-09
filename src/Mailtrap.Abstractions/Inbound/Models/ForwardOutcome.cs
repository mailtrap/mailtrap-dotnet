namespace Mailtrap.Inbound.Models;


/// <summary>
/// Represents the outcome of forwarding a received message to one forward-rule destination.
/// </summary>
public sealed record ForwardOutcome
{
    /// <summary>
    /// Gets or sets the forward rule identifier.
    /// </summary>
    ///
    /// <value>
    /// Forward rule identifier.
    /// </value>
    [JsonPropertyName("rule_id")]
    public long RuleId { get; set; }

    /// <summary>
    /// Gets or sets the forward rule name.
    /// </summary>
    ///
    /// <value>
    /// Forward rule name.
    /// </value>
    [JsonPropertyName("rule_name")]
    public string? RuleName { get; set; }

    /// <summary>
    /// Gets or sets the destination address.
    /// </summary>
    ///
    /// <value>
    /// Destination address.
    /// </value>
    [JsonPropertyName("destination")]
    public string? Destination { get; set; }

    /// <summary>
    /// Gets or sets the forward status.
    /// </summary>
    ///
    /// <value>
    /// Forward status.
    /// </value>
    [JsonPropertyName("status")]
    public ForwardOutcomeStatus Status { get; set; } = ForwardOutcomeStatus.Unknown;

    /// <summary>
    /// Gets or sets the rejection reason.
    /// </summary>
    ///
    /// <value>
    /// Rejection reason.
    /// </value>
    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    /// <summary>
    /// Gets or sets the forwarded message identifier.
    /// </summary>
    ///
    /// <value>
    /// Forwarded message identifier.
    /// </value>
    [JsonPropertyName("message_id")]
    public string? MessageId { get; set; }
}
