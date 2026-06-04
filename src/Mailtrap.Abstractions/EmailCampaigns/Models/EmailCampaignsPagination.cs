namespace Mailtrap.EmailCampaigns.Models;


/// <summary>
/// Represents page-token pagination metadata for a list of email campaigns.
/// </summary>
public sealed record EmailCampaignsPagination
{
    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    ///
    /// <value>
    /// Current page number.
    /// </value>
    [JsonPropertyName("token")]
    [JsonPropertyOrder(1)]
    public int Token { get; set; }

    /// <summary>
    /// Gets or sets the previous page number.
    /// </summary>
    ///
    /// <value>
    /// Previous page number, or <see langword="null"/> on the first page.
    /// </value>
    [JsonPropertyName("prev_token")]
    [JsonPropertyOrder(2)]
    public int? PrevToken { get; set; }

    /// <summary>
    /// Gets or sets the next page number.
    /// </summary>
    ///
    /// <value>
    /// Next page number, or <see langword="null"/> on the last page.
    /// </value>
    [JsonPropertyName("next_token")]
    [JsonPropertyOrder(3)]
    public int? NextToken { get; set; }

    /// <summary>
    /// Gets or sets the URL of the first page.
    /// </summary>
    ///
    /// <value>
    /// URL of the first page.
    /// </value>
    [JsonPropertyName("first_url")]
    [JsonPropertyOrder(4)]
    public Uri? FirstUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL of the previous page.
    /// </summary>
    ///
    /// <value>
    /// URL of the previous page, or <see langword="null"/> on the first page.
    /// </value>
    [JsonPropertyName("prev_url")]
    [JsonPropertyOrder(5)]
    public Uri? PrevUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL of the current page.
    /// </summary>
    ///
    /// <value>
    /// URL of the current page.
    /// </value>
    [JsonPropertyName("current_url")]
    [JsonPropertyOrder(6)]
    public Uri? CurrentUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL of the next page.
    /// </summary>
    ///
    /// <value>
    /// URL of the next page, or <see langword="null"/> on the last page.
    /// </value>
    [JsonPropertyName("next_url")]
    [JsonPropertyOrder(7)]
    public Uri? NextUrl { get; set; }
}
