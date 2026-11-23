namespace Mailtrap.SubscriptionTopics.Models;


/// <summary>
/// Represents a paginated page of subscription topics.
/// </summary>
public sealed record SubscriptionTopicList
{
    /// <summary>
    /// Gets the page of subscription topics, ordered by identifier.
    /// </summary>
    ///
    /// <value>
    /// Page of subscription topics. Deleted topics are left out.
    /// </value>
    [JsonPropertyName("data")]
    [JsonPropertyOrder(1)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<SubscriptionTopic> Data { get; } = [];

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
