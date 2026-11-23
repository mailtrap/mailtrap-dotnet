namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Email log message event for event_type = delivery.
/// </summary>
public sealed record EmailLogMessageEventDelivery : EmailLogMessageEvent
{
    /// <summary>
    /// Value of <c>event_type</c> for this event.
    /// </summary>
    public const string Discriminator = "delivery";

    /// <inheritdoc />
    [JsonPropertyName("event_type")]
    [JsonPropertyOrder(-1)]
    public override string EventType => Discriminator;

    /// <summary>
    /// Delivery-specific details.
    /// </summary>
    [JsonPropertyName("details")]
    public EventDetailsDelivery? Details { get; set; }
}
