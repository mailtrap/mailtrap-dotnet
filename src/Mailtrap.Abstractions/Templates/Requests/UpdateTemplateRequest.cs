namespace Mailtrap.Templates.Requests;


/// <summary>
/// Request object for updating a template.<br/>
/// The request body is sent flat (no envelope). All properties are optional -
/// only properties set on the request are sent and changed.
/// </summary>
public sealed record UpdateTemplateRequest : IValidatable
{
    /// <summary>
    /// Gets or sets the template name.
    /// </summary>
    /// <remarks>
    /// Template name must be no longer than 255 characters.
    /// </remarks>
    /// <value>
    /// Template name, or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the template category.
    /// </summary>
    /// <remarks>
    /// Template category must be no longer than 255 characters.
    /// </remarks>
    /// <value>
    /// Template category, or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("category")]
    [JsonPropertyOrder(2)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets the template subject.
    /// </summary>
    /// <remarks>
    /// Template subject must be no longer than 255 characters.
    /// </remarks>
    /// <value>
    /// Template subject, or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("subject")]
    [JsonPropertyOrder(3)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Subject { get; set; }

    /// <summary>
    /// Gets or sets the template body HTML.
    /// </summary>
    /// <remarks>
    /// Template body HTML must be no longer than 10_000_000 characters.
    /// </remarks>
    /// <value>
    /// Template body HTML, or <see langword="null"/> to leave unchanged.
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
    /// Template body text, or <see langword="null"/> to leave unchanged.
    /// </value>
    [JsonPropertyName("body_text")]
    [JsonPropertyOrder(5)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BodyText { get; set; }


    /// <inheritdoc/>
    public ValidationResult Validate()
    {
        return UpdateTemplateRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
