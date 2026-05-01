namespace Mailtrap.ApiTokens.Requests;


/// <summary>
/// Request to create a new API token.
/// </summary>
public sealed record CreateApiTokenRequest : IValidatable
{
    /// <summary>
    /// Gets or sets the API token display name.
    /// </summary>
    ///
    /// <value>
    /// API token display name.
    /// </value>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(1)]
    [JsonRequired]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the resource accesses to grant to the API token.
    /// </summary>
    ///
    /// <value>
    /// Collection of resource accesses.
    /// </value>
    [JsonPropertyName("resources")]
    [JsonPropertyOrder(2)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<ApiTokenAccessRequest> Resources { get; } = [];


    /// <inheritdoc/>
    public ValidationResult Validate()
    {
        return CreateApiTokenRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
