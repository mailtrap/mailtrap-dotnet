namespace Mailtrap.Emails.Models;


internal sealed class EmailAddressValidator : AbstractValidator<EmailAddress>
{
    public static EmailAddressValidator Instance { get; } = new();

    public EmailAddressValidator()
    {
        RuleFor(r => r)
            .NotNull();

        RuleFor(r => r.Email)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .EmailAddress();
    }
}
