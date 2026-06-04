namespace Mailtrap.EmailCampaigns.Requests;


/// <summary>
/// Request DTO that wraps <see cref="UpdateEmailCampaignRequest"/> in the <c>email_campaign</c> envelope expected by the API.
/// </summary>
internal sealed record UpdateEmailCampaignRequestDto : IValidatable
{
    [JsonPropertyName("email_campaign")]
    [JsonPropertyOrder(1)]
    public UpdateEmailCampaignRequest EmailCampaign { get; }


    public UpdateEmailCampaignRequestDto(UpdateEmailCampaignRequest emailCampaign)
    {
        Ensure.NotNull(emailCampaign, nameof(emailCampaign));

        EmailCampaign = emailCampaign;
    }


    public ValidationResult Validate() => EmailCampaign.Validate();
}
