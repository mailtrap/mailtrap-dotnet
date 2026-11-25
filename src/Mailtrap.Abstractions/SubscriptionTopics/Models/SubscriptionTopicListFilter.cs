namespace Mailtrap.SubscriptionTopics.Models;


/// <summary>
/// Represents a set of pagination parameters for listing subscription topics.
/// </summary>
public sealed record SubscriptionTopicListFilter
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
    /// Gets or sets the number of subscription topics per page.
    /// </summary>
    ///
    /// <value>
    /// Number of subscription topics per page. Defaults to <c>50</c> and is capped at <c>100</c> by the API.
    /// </value>
    public int? PerPage { get; set; }
}
