namespace Mailtrap.EmailCampaigns.Requests;


internal static class EmailCampaignRequestExtensions
{
    public static CreateEmailCampaignRequestDto ToDto(this CreateEmailCampaignRequest request) => new(request);

    public static UpdateEmailCampaignRequestDto ToDto(this UpdateEmailCampaignRequest request) => new(request);
}
