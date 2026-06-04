namespace Mailtrap.EmailCampaigns.Requests;


/// <summary>
/// Request DTO that wraps <see cref="CreateEmailCampaignRequest"/> in the <c>email_campaign</c> envelope expected by the API.
/// </summary>
internal sealed record CreateEmailCampaignRequestDto : IValidatable
{
    [JsonPropertyName("email_campaign")]
    [JsonPropertyOrder(1)]
    public CreateEmailCampaignRequest EmailCampaign { get; }


    public CreateEmailCampaignRequestDto(CreateEmailCampaignRequest emailCampaign)
    {
        Ensure.NotNull(emailCampaign, nameof(emailCampaign));

        EmailCampaign = emailCampaign;
    }


    public ValidationResult Validate() => EmailCampaign.Validate();
}
