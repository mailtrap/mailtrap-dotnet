namespace Mailtrap.Templates.Validators;


/// <summary>
/// Validator for <see cref="UpdateTemplateRequest"/>.<br/>
/// All fields are optional; ensures provided Name, Category, Subject are not empty and do not exceed 255 characters each,
/// and provided BodyHtml and BodyText do not exceed 10,000,000 characters each.
/// </summary>
public sealed class UpdateTemplateRequestValidator : AbstractValidator<UpdateTemplateRequest>
{
    /// <summary>
    /// Static validator instance for reuse.
    /// </summary>
    public static UpdateTemplateRequestValidator Instance { get; } = new();

    /// <summary>
    /// Primary constructor.
    /// </summary>
    public UpdateTemplateRequestValidator()
    {
        When(r => r.Name is not null, () => RuleFor(r => r.Name).NotEmpty().MaximumLength(255));
        When(r => r.Category is not null, () => RuleFor(r => r.Category).NotEmpty().MaximumLength(255));
        When(r => r.Subject is not null, () => RuleFor(r => r.Subject).NotEmpty().MaximumLength(255));
        When(r => r.BodyHtml is not null, () => RuleFor(r => r.BodyHtml).MaximumLength(10_000_000));
        When(r => r.BodyText is not null, () => RuleFor(r => r.BodyText).MaximumLength(10_000_000));
    }
}
