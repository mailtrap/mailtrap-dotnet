namespace Mailtrap.SubscriptionTopics.Responses;


/// <summary>
/// Response DTO that unwraps a single subscription topic from the <c>data</c> envelope.
/// </summary>
internal sealed record SubscriptionTopicResponseDto
{
    [JsonPropertyName("data")]
    [JsonPropertyOrder(1)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public SubscriptionTopic SubscriptionTopic { get; } = new();
}
