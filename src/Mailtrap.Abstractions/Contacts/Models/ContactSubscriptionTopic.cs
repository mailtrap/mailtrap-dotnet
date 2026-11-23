namespace Mailtrap.Contacts.Models;


/// <summary>
/// Represents a contact's subscription status for one subscription topic on one sending domain.
/// </summary>
///
/// <remarks>
/// Used both in contact responses and in contact create, update and import requests.
/// In a request, pairs of topic and domain left out keep their current status.
/// </remarks>
public sealed record ContactSubscriptionTopic
{
    /// <summary>
    /// Gets or sets the identifier of the subscription topic,
    /// as returned by the Subscription Topics endpoints.
    /// </summary>
    ///
    /// <value>
    /// Subscription topic identifier.
    /// </value>
    [JsonPropertyName("subscription_topic_id")]
    [JsonPropertyOrder(1)]
    public long SubscriptionTopicId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the sending domain the status applies to,
    /// as returned by the Sending Domains endpoints.
    /// </summary>
    ///
    /// <value>
    /// Sending domain identifier.
    /// </value>
    [JsonPropertyName("domain_id")]
    [JsonPropertyOrder(2)]
    public long DomainId { get; set; }

    /// <summary>
    /// Gets or sets the contact's subscription status for the topic on the domain.
    /// </summary>
    ///
    /// <value>
    /// <see cref="ContactStatus.Subscribed"/> or <see cref="ContactStatus.Unsubscribed"/>.
    /// </value>
    [JsonPropertyName("status")]
    [JsonPropertyOrder(3)]
    public ContactStatus Status { get; set; } = ContactStatus.Unknown;
}
