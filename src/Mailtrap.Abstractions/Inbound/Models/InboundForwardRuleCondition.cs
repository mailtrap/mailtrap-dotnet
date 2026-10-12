namespace Mailtrap.Inbound.Models;


/// <summary>
/// Represents an inbound forward rule condition.
/// </summary>
public sealed record InboundForwardRuleCondition
{
    /// <summary>
    /// Gets or sets the match type.
    /// </summary>
    ///
    /// <value>
    /// Match type.
    /// </value>
    [JsonPropertyName("match_type")]
    public ForwardRuleMatchType? MatchType { get; set; }

    /// <summary>
    /// Gets or sets the comparison operator.
    /// </summary>
    ///
    /// <value>
    /// Comparison operator.
    /// </value>
    [JsonPropertyName("operator")]
    public ForwardRuleOperator? Operator { get; set; }

    /// <summary>
    /// Gets or sets the condition value.
    /// </summary>
    ///
    /// <value>
    /// Condition value.
    /// </value>
    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the header name.
    /// </summary>
    ///
    /// <value>
    /// Header name.
    /// </value>
    [JsonPropertyName("header_key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? HeaderKey { get; set; }
}
