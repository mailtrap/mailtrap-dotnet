namespace Mailtrap.Organizations.Requests;


/// <summary>
/// Request to create a new sub account under an organization. Wraps the attributes
/// in an <c>account</c> envelope, matching the API contract.
/// </summary>
public sealed record CreateSubAccountRequest : IValidatable
{
    /// <summary>
    /// Gets or sets the sub account attributes.
    /// </summary>
    [JsonPropertyName("account")]
    [JsonPropertyOrder(1)]
    [JsonRequired]
    public SubAccountAttributes Account { get; set; } = new();


    /// <inheritdoc/>
    public ValidationResult Validate()
    {
        return CreateSubAccountRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
