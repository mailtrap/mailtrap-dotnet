namespace Mailtrap.Inbound.Requests;


/// <summary>
/// Request object for updating an inbound forward rule.
/// </summary>
public sealed record UpdateInboundForwardRuleRequest : IValidatable
{
    /// <summary>
    /// Gets or sets the rule name.
    /// </summary>
    ///
    /// <value>
    /// Rule name.
    /// </value>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the rule conditions.
    /// </summary>
    ///
    /// <value>
    /// Rule conditions.
    /// </value>
    [JsonPropertyName("conditions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<InboundForwardRuleCondition>? Conditions { get; set; }

    /// <summary>
    /// Gets or sets the rule destinations.
    /// </summary>
    ///
    /// <value>
    /// Rule destinations.
    /// </value>
    [JsonPropertyName("destinations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<InboundForwardRuleDestination>? Destinations { get; set; }


    /// <inheritdoc/>
    public ValidationResult Validate()
    {
        return UpdateInboundForwardRuleRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
