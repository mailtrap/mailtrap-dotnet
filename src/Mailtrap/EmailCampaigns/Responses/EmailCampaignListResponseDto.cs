namespace Mailtrap.EmailCampaigns.Responses;


/// <summary>
/// Response DTO that unwraps the page of email campaigns from the <c>data</c> envelope
/// and its pagination metadata.
/// </summary>
internal sealed record EmailCampaignListResponseDto
{
    [JsonPropertyName("data")]
    [JsonPropertyOrder(1)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<EmailCampaign> Data { get; } = [];

    [JsonPropertyName("pagination")]
    [JsonPropertyOrder(2)]
    public EmailCampaignsPagination? Pagination { get; set; }


    public EmailCampaignList FromDto()
    {
        var result = new EmailCampaignList { Pagination = Pagination };

        foreach (var campaign in Data)
        {
            result.Data.Add(campaign);
        }

        return result;
    }
}
