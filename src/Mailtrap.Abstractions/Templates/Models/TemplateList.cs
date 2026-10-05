namespace Mailtrap.Templates.Models;


/// <summary>
/// Represents a paginated page of templates.
/// </summary>
public sealed record TemplateList
{
    /// <summary>
    /// Gets the page of templates.
    /// </summary>
    ///
    /// <value>
    /// Page of templates.
    /// </value>
    [JsonPropertyName("data")]
    [JsonPropertyOrder(1)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<Template> Data { get; } = [];

    /// <summary>
    /// Gets or sets the page-token pagination metadata.
    /// </summary>
    ///
    /// <value>
    /// Pagination metadata, or <see langword="null"/> when not present.
    /// </value>
    [JsonPropertyName("pagination")]
    [JsonPropertyOrder(2)]
    public Pagination? Pagination { get; set; }
}
