namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Event details for event_type = topic_unsubscribe or topic_resubscribe.
/// </summary>
public sealed record EventDetailsTopicSubscription
{
    /// <summary>
    /// Web IP address that changed the subscription.
    /// </summary>
    [JsonPropertyName("web_ip_address")]
    public string? WebIpAddress { get; set; }

    /// <summary>
    /// Identifier of the subscription topic, as returned by the Subscription Topics endpoints.
    /// </summary>
    [JsonPropertyName("subscription_topic_id")]
    public long? SubscriptionTopicId { get; set; }
}
