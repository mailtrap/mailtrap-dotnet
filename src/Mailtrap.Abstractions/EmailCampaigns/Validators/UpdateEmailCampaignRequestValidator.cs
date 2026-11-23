namespace Mailtrap.EmailCampaigns.Validators;


/// <summary>
/// Validator for <see cref="UpdateEmailCampaignRequest"/>.<br/>
/// All fields are optional; ensures a provided sending domain identifier is positive.
/// </summary>
public sealed class UpdateEmailCampaignRequestValidator : AbstractValidator<UpdateEmailCampaignRequest>
{
    /// <summary>
    /// Static validator instance for reuse.
    /// </summary>
    public static UpdateEmailCampaignRequestValidator Instance { get; } = new();

    /// <summary>
    /// Primary constructor.
    /// </summary>
    public UpdateEmailCampaignRequestValidator()
    {
        When(r => r.DomainId is not null, () =>
        {
            RuleFor(r => r.DomainId)
                .GreaterThan(0)
                .WithMessage("'DomainId' must be a positive sending domain ID.");
        });
    }
}
