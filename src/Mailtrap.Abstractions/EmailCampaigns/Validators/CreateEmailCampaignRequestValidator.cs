namespace Mailtrap.EmailCampaigns.Validators;


/// <summary>
/// Validator for <see cref="CreateEmailCampaignRequest"/>.<br/>
/// Ensures the campaign name, sending domain identifier, From local part and template subject are provided.
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

        RuleFor(r => r.DomainId)
            .NotNull()
            .WithMessage("'DomainId' must not be empty.")
            .GreaterThan(0)
            .WithMessage("'DomainId' must be a positive sending domain ID.");

        RuleFor(r => r.FromLocalPart)
            .NotEmpty()
            .WithMessage("'FromLocalPart' must not be empty.");

        RuleFor(r => r.TemplateAttributes)
            .NotNull()
            .WithMessage("'TemplateAttributes' must be provided.");

        When(r => r.TemplateAttributes is not null, () =>
        {
            RuleFor(r => r.TemplateAttributes!.Subject)
                .NotEmpty()
                .WithMessage("'TemplateAttributes.Subject' must not be empty.");
        });
    }
}
