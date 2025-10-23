namespace Mailtrap.Emails.Validators;


/// <summary>
/// Represents validator for <see cref="SendEmailRequest"/>.
/// Ensures that count of recipients in To, Cc and Bcc does not exceed 1000 in total
/// and that at least one recipient is specified in either of them.
/// Also applies <see cref="EmailRequestValidator"/> to validate other properties.
/// </summary>
internal sealed class SendEmailRequestValidator : AbstractValidator<SendEmailRequest>
{
    public static SendEmailRequestValidator Instance { get; } = new();

    public SendEmailRequestValidator()
    {
        RuleFor(r => r)
            .SetValidator(EmailRequestValidator.Instance);

        RuleFor(r => r)
            .SetValidator(SendEmailRecipientsValidator.Instance);
    }
}
