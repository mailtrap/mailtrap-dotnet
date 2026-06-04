namespace Mailtrap.EmailCampaigns.Validators;


/// <summary>
/// Validator for <see cref="CreateEmailCampaignRequest"/>.<br/>
/// Ensures the campaign name and sending domain identifier are provided.
/// </summary>
public sealed class CreateEmailCampaignRequestValidator : AbstractValidator<CreateEmailCampaignRequest>
{
    /// <summary>
    /// Static validator instance for reuse.
    /// </summary>
    public static CreateEmailCampaignRequestValidator Instance { get; } = new();

    /// <summary>
    /// Primary constructor.
    /// </summary>
    public CreateEmailCampaignRequestValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage("'Name' must not be empty.");

        RuleFor(r => r.MailsendDomainId)
            .GreaterThan(0)
            .WithMessage("'MailsendDomainId' must be set to a valid sending domain identifier.");
    }
}
