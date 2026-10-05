namespace Mailtrap.Inbound.Models;


/// <summary>
/// Represents an inbound forward rule.
/// </summary>
public sealed record InboundForwardRule
{
    /// <summary>
    /// Gets or sets the forward rule identifier.
    /// </summary>
    ///
    /// <value>
    /// Forward rule identifier.
    /// </value>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the forward rule name.
    /// </summary>
    ///
    /// <value>
    /// Forward rule name.
    /// </value>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the rule was created.
    /// </summary>
    ///
    /// <value>
    /// Timestamp when the rule was created.
    /// </value>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the rule was last updated.
    /// </summary>
    ///
    /// <value>
    /// Timestamp when the rule was last updated.
    /// </value>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Gets the rule conditions.
    /// </summary>
    ///
    /// <value>
    /// Rule conditions.
    /// </value>
    [JsonPropertyName("conditions")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<InboundForwardRuleCondition> Conditions { get; } = [];

    /// <summary>
    /// Gets the rule destinations.
    /// </summary>
    ///
    /// <value>
    /// Rule destinations.
    /// </value>
    [JsonPropertyName("destinations")]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<InboundForwardRuleDestination> Destinations { get; } = [];
}
