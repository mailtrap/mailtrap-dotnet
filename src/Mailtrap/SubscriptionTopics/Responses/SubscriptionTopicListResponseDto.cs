namespace Mailtrap.SubscriptionTopics.Responses;


/// <summary>
/// Response DTO that unwraps the page of subscription topics from the <c>data</c> envelope
/// and its pagination metadata.
/// </summary>
internal sealed record SubscriptionTopicListResponseDto
{
    [JsonPropertyName("data")]
    [JsonPropertyOrder(1)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<SubscriptionTopic> Data { get; } = [];

    [JsonPropertyName("pagination")]
    [JsonPropertyOrder(2)]
    public Pagination? Pagination { get; set; }


    public SubscriptionTopicList FromDto()
    {
        var result = new SubscriptionTopicList { Pagination = Pagination };

        foreach (var subscriptionTopic in Data)
        {
            result.Data.Add(subscriptionTopic);
        }

        return result;
    }
}
