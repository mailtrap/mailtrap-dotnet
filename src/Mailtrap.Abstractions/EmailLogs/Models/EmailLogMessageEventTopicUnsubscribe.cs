namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = topic_unsubscribe: the recipient unsubscribed from a single subscription topic.
/// </summary>
public sealed record EmailLogMessageEventTopicUnsubscribe : EmailLogMessageEvent
{
    /// <inheritdoc />
    public override string EventType => "topic_unsubscribe";

    /// <summary>
    /// Topic unsubscribe-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsTopicSubscription? Details { get; set; }
}
