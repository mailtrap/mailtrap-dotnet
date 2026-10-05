namespace Mailtrap.Templates.Validators;


/// <summary>
/// Validator for <see cref="CreateTemplateRequest"/>.<br/>
/// Ensures Name, Category, Subject are not empty and do not exceed 255 characters each,
/// and BodyHtml and BodyText do not exceed 10,000,000 characters each if provided.
/// </summary>
public sealed class CreateTemplateRequestValidator : AbstractValidator<CreateTemplateRequest>
{
    /// <summary>
    /// Static validator instance for reuse.
    /// </summary>
    public static CreateTemplateRequestValidator Instance { get; } = new();

    /// <summary>
    /// Primary constructor.
    /// </summary>
    public CreateTemplateRequestValidator()
    {
        RuleFor(r => r.Name).NotEmpty().MaximumLength(255);
        RuleFor(r => r.Category).NotEmpty().MaximumLength(255);
        RuleFor(r => r.Subject).NotEmpty().MaximumLength(255);
        RuleFor(r => r.BodyHtml).MaximumLength(10_000_000);
        RuleFor(r => r.BodyText).MaximumLength(10_000_000);
    }
}
