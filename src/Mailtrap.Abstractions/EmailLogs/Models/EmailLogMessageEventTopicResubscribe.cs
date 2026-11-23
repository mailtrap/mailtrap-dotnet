namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = topic_resubscribe: the recipient subscribed again to a single subscription topic.
/// </summary>
public sealed record EmailLogMessageEventTopicResubscribe : EmailLogMessageEvent
{
    /// <summary>
    /// Value of <c>event_type</c> for this event.
    /// </summary>
    public const string Discriminator = "topic_resubscribe";

    /// <inheritdoc />
    [JsonPropertyName("event_type")]
    [JsonPropertyOrder(-1)]
    public override string EventType => Discriminator;

    /// <summary>
    /// Topic resubscribe-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsTopicSubscription? Details { get; set; }
}
