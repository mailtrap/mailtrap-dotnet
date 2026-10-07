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
    /// Number of templates per page. The API clamps the value rather than rejecting it:
    /// below <c>1</c> becomes <c>50</c> and above <c>100</c> becomes <c>100</c>.
    /// </value>
    public int? PerPage { get; set; }
}
