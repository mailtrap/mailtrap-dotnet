namespace Mailtrap.Emails.Validators;


/// <summary>
/// Represents validator for <see cref="EmailRequest"/>.
/// Ensures that From is set, that either of TextBody or HtmlBody<br/>
/// is set when TemplateId is not set, and that Subject is not set when TemplateId is set.
/// Also validates Attachments and ReplyTo if they are set.
/// Provides two instances: one for single email requests and another<br/>
/// for <see cref="BatchEmailRequest.Base"/> which has no required properties.
/// </summary>
internal sealed class EmailRequestValidator : AbstractValidator<EmailRequest>
{
    public static EmailRequestValidator Instance { get; } = new();
    public static EmailRequestValidator BaseInstance { get; } = new(true);

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailRequestValidator"/> class.
    /// If <paramref name="isBase"/> is true, the validator is configured for Base request validation.
    /// This means that the no properties are required and perform basic validation.
    /// </summary>
    /// <param name="isBase">if <c>true</c>, the validator is configured for batch's Base request.</param>
    public EmailRequestValidator(bool isBase = false)
    {
        RuleFor(r => r.From!)
            .SetValidator(EmailAddressValidator.Instance)
            .When(r => r.From is not null);

        RuleFor(r => r.From!)
            .NotNull()
            .When(r => !isBase);

        RuleFor(r => r.ReplyTo!)
            .SetValidator(EmailAddressValidator.Instance)
            .When(r => r.ReplyTo is not null);

        RuleForEach(r => r.Attachments)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .SetValidator(AttachmentValidator.Instance);

        RuleFor(r => r.Category)
            .MaximumLength(255);

        When(r => string.IsNullOrEmpty(r.TemplateId), () =>
        {
            RuleFor(r => r.Subject)
                .NotNull()
                .MinimumLength(1)
                .When(r => !isBase);

            RuleFor(r => r.TextBody)
                .NotNull()
                .MinimumLength(1)
                .When(r => string.IsNullOrEmpty(r.HtmlBody) && !isBase)
                .WithMessage($"Either '{nameof(EmailRequest.TextBody)}' or '{nameof(EmailRequest.HtmlBody)}' or both should be set to non-empty string, when template is not specified.");

            RuleFor(r => r.HtmlBody)
                .NotNull()
                .MinimumLength(1)
                .When(r => string.IsNullOrEmpty(r.TextBody) && !isBase)
                .WithMessage($"Either '{nameof(EmailRequest.TextBody)}' or '{nameof(EmailRequest.HtmlBody)}' or both should be set to non-empty string, when template is not specified.");
        });

        When(r => !string.IsNullOrEmpty(r.TemplateId), () =>
        {
            RuleFor(r => r.Subject)
                .Null()
                .WithMessage($"'{nameof(EmailRequest.Subject)}' should be null, when '{nameof(EmailRequest.TemplateId)}' is specified.");

            RuleFor(r => r.TextBody)
                .Null()
                .WithMessage($"'{nameof(EmailRequest.TextBody)}' should be null, when '{nameof(EmailRequest.TemplateId)}' is specified.");

            RuleFor(r => r.HtmlBody)
                .Null()
                .WithMessage($"'{nameof(EmailRequest.HtmlBody)}' should be null, when '{nameof(EmailRequest.TemplateId)}' is specified.");
        });
    }
}
