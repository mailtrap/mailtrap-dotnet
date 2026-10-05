namespace Mailtrap.Templates.Models;


/// <summary>
/// Represents a set of pagination parameters for listing templates.
/// </summary>
public sealed record TemplateListFilter
{
    /// <summary>
    /// Gets or sets the page number to retrieve (page-token pagination).
    /// </summary>
    ///
    /// <value>
    /// Page number to retrieve. Defaults to <c>1</c> when not specified.
    /// </value>
    public int? Token { get; set; }

    /// <summary>
    /// Gets or sets the number of templates per page.
    /// </summary>
    ///
    /// <value>
    /// Number of templates per page. Defaults to <c>50</c> and is capped at <c>100</c> by the API.
    /// </value>
    public int? PerPage { get; set; }
}
