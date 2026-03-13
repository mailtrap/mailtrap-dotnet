namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = delivery.
/// </summary>
public sealed record EmailLogMessageEventDelivery : EmailLogMessageEvent
{
    /// <inheritdoc />
    public override string EventType => "delivery";

    /// <summary>
    /// Delivery-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsDelivery? Details { get; set; }
}
