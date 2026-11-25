namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = topic_unsubscribe: the recipient unsubscribed from a single subscription topic.
/// </summary>
public sealed record EmailLogMessageEventTopicUnsubscribe : EmailLogMessageEvent
{
    /// <summary>
    /// Value of <c>event_type</c> for this event.
    /// </summary>
    public const string Discriminator = "topic_unsubscribe";

    /// <inheritdoc />
    [JsonPropertyName("event_type")]
    [JsonPropertyOrder(-1)]
    public override string EventType => Discriminator;

    /// <summary>
    /// Topic unsubscribe-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsTopicSubscription? Details { get; set; }
}
