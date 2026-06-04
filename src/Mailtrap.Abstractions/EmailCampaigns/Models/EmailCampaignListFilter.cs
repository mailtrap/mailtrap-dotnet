namespace Mailtrap.EmailCampaigns.Models;


/// <summary>
/// Represents a set of filtering and pagination parameters for listing email campaigns.
/// </summary>
public sealed record EmailCampaignListFilter
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
    /// Gets or sets the number of campaigns per page.
    /// </summary>
    ///
    /// <value>
    /// Number of campaigns per page. Defaults to <c>50</c> and is capped at <c>100</c> by the API.
    /// </value>
    public int? PerPage { get; set; }

    /// <summary>
    /// Gets or sets the campaign name filter (case-insensitive partial match).
    /// </summary>
    ///
    /// <value>
    /// Name filter, or <see langword="null"/> for no filtering.
    /// </value>
    ///
    /// <remarks>
    /// Serialized to the <c>search</c> query parameter.
    /// </remarks>
    public string? Search { get; set; }
}
