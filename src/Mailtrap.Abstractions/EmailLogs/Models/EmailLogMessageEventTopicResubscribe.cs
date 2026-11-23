namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = topic_resubscribe: the recipient subscribed again to a single subscription topic.
/// </summary>
public sealed record EmailLogMessageEventTopicResubscribe : EmailLogMessageEvent
{
    /// <inheritdoc />
    [JsonPropertyName("event_type")]
    [JsonPropertyOrder(-1)]
    public override string EventType => "topic_resubscribe";

    /// <summary>
    /// Topic resubscribe-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsTopicSubscription? Details { get; set; }
}
