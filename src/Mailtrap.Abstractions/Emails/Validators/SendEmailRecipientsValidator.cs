namespace Mailtrap.Emails.Validators;


/// <summary>
/// Represents validator for <see cref="SendEmailRequest"/> which
/// validates ONLY email recipients: To, Cc, Bcc.<br/>
/// Ensures that there is at least one recipient in either of To, Cc or Bcc
/// and that there are no more than 1000 recipients in those lists.
/// </summary>
internal sealed class SendEmailRecipientsValidator : AbstractValidator<SendEmailRequest>
{
    public static SendEmailRecipientsValidator Instance { get; } = new();

    public SendEmailRecipientsValidator()
    {
        RuleFor(r => r.To)
            .Must(r => r is null || r.Count is <= 1000);
        RuleForEach(r => r.To)
            .NotNull()
            .SetValidator(EmailAddressValidator.Instance);

        RuleFor(r => r.Cc)
            .Must(r => r is null || r.Count is <= 1000);
        RuleForEach(r => r.Cc)
            .NotNull()
            .SetValidator(EmailAddressValidator.Instance);

        RuleFor(r => r.Bcc)
            .Must(r => r is null || r.Count is <= 1000);
        RuleForEach(r => r.Bcc)
            .NotNull()
            .SetValidator(EmailAddressValidator.Instance);

        RuleFor(r => r)
            .Must(r => (r.To?.Count ?? 0) + (r.Cc?.Count ?? 0) + (r.Bcc?.Count ?? 0) is > 0)
            .WithName("Recipients")
            .WithMessage("There should be at least one email recipient added to either To, Cc or Bcc.");

        RuleFor(r => r)
            .Must(r => (r.To?.Count ?? 0) + (r.Cc?.Count ?? 0) + (r.Bcc?.Count ?? 0) is <= 1000)
            .WithName("Recipients")
            .WithMessage("The total number of email recipients in To, Cc and Bcc must not exceed 1000.");
    }
}
