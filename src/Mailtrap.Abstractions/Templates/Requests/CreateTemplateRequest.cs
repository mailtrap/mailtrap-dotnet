namespace Mailtrap.Templates.Requests;


/// <summary>
/// Request object for creating a template.<br/>
/// The request body is sent flat (no envelope).
/// </summary>
public sealed record CreateTemplateRequest : IValidatable
{
    /// <summary>
    /// Gets or sets the template name. Required.
    /// </summary>
    /// <remarks>
    /// Template name must be no longer than 255 characters.
    /// </remarks>
    /// <value>
    /// Template name.
    /// </value>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(1)]
    [JsonRequired]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the template category. Required.
    /// </summary>
    /// <remarks>
    /// Template category must be no longer than 255 characters.
    /// </remarks>
    /// <value>
    /// Template category.
    /// </value>
    [JsonPropertyName("category")]
    [JsonPropertyOrder(2)]
    [JsonRequired]
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets the template subject. Required.
    /// </summary>
    /// <remarks>
    /// Template subject must be no longer than 255 characters.
    /// </remarks>
    /// <value>
    /// Template subject.
    /// </value>
    [JsonPropertyName("subject")]
    [JsonPropertyOrder(3)]
    [JsonRequired]
    public string? Subject { get; set; }

    /// <summary>
    /// Gets or sets the template body HTML.
    /// </summary>
    /// <remarks>
    /// Template body HTML must be no longer than 10_000_000 characters.
    /// </remarks>
    /// <value>
    /// Template body HTML, or <see langword="null"/> to omit it.
    /// </value>
    [JsonPropertyName("body_html")]
    [JsonPropertyOrder(4)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BodyHtml { get; set; }

    /// <summary>
    /// Gets or sets the template body text.
    /// </summary>
    /// <remarks>
    /// Template body text must be no longer than 10_000_000 characters.
    /// </remarks>
    /// <value>
    /// Template body text, or <see langword="null"/> to omit it.
    /// </value>
    [JsonPropertyName("body_text")]
    [JsonPropertyOrder(5)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BodyText { get; set; }


    /// <inheritdoc/>
    public ValidationResult Validate()
    {
        return CreateTemplateRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
