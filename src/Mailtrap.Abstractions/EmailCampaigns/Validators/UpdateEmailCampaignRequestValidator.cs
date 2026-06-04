namespace Mailtrap.EmailCampaigns.Validators;


/// <summary>
/// Validator for <see cref="UpdateEmailCampaignRequest"/>.<br/>
/// All fields are optional; ensures provided values are consistent
/// (a positive sending domain identifier and a scheduled time when scheduling).
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
        When(r => r.MailsendDomainId is not null, () =>
        {
            RuleFor(r => r.MailsendDomainId)
                .GreaterThan(0)
                .WithMessage("'MailsendDomainId' must be set to a valid sending domain identifier.");
        });

        When(r => r.DeliveryMode == DeliveryMode.Scheduled, () =>
        {
            RuleFor(r => r.ScheduledFor)
                .NotNull()
                .WithMessage("'ScheduledFor' is required when 'DeliveryMode' is 'scheduled'.");
        });
    }
}
