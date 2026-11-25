namespace Mailtrap.SubscriptionTopics.Models;


/// <summary>
/// Represents a subscription topic: a kind of email that contacts can subscribe to or unsubscribe from.
/// </summary>
public sealed record SubscriptionTopic
{
    /// <summary>
    /// Gets or sets the subscription topic identifier.
    /// </summary>
    ///
    /// <value>
    /// Subscription topic identifier.
    /// </value>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(1)]
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the subscription topic name.
    /// </summary>
    ///
    /// <value>
    /// Subscription topic name.
    /// </value>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(2)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the subscription topic description.
    /// </summary>
    ///
    /// <value>
    /// Subscription topic description, or <see langword="null"/> when it has none.
    /// </value>
    [JsonPropertyName("description")]
    [JsonPropertyOrder(3)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the sending domain the topic covers,
    /// as returned by the Sending Domains endpoints.
    /// </summary>
    ///
    /// <value>
    /// Sending domain identifier, or <see langword="null"/> when the topic covers every domain.
    /// </value>
    [JsonPropertyName("domain_id")]
    [JsonPropertyOrder(4)]
    public long? DomainId { get; set; }

    /// <summary>
    /// Gets or sets the date and time the topic was deleted.
    /// </summary>
    ///
    /// <value>
    /// Deletion timestamp, or <see langword="null"/> when the topic is not deleted.
    /// </value>
    ///
    /// <remarks>
    /// The topic list leaves deleted topics out; getting a topic by its identifier returns it with this set.
    /// </remarks>
    [JsonPropertyName("deleted_at")]
    [JsonPropertyOrder(5)]
    public DateTimeOffset? DeletedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time the topic was created.
    /// </summary>
    ///
    /// <value>
    /// Creation timestamp.
    /// </value>
    [JsonPropertyName("created_at")]
    [JsonPropertyOrder(6)]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time the topic was last updated.
    /// </summary>
    ///
    /// <value>
    /// Last update timestamp.
    /// </value>
    [JsonPropertyName("updated_at")]
    [JsonPropertyOrder(7)]
    public DateTimeOffset? UpdatedAt { get; set; }
}
