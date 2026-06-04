namespace Mailtrap.EmailCampaigns.Models;


/// <summary>
/// Represents a paginated page of email campaigns.
/// </summary>
public sealed record EmailCampaignList
{
    /// <summary>
    /// Gets the page of email campaigns, newest first.
    /// </summary>
    ///
    /// <value>
    /// Page of email campaigns.
    /// </value>
    [JsonPropertyName("data")]
    [JsonPropertyOrder(1)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<EmailCampaign> Data { get; } = [];

    /// <summary>
    /// Gets or sets the page-token pagination metadata.
    /// </summary>
    ///
    /// <value>
    /// Pagination metadata, or <see langword="null"/> when not present.
    /// </value>
    [JsonPropertyName("pagination")]
    [JsonPropertyOrder(2)]
    public EmailCampaignsPagination? Pagination { get; set; }
}
