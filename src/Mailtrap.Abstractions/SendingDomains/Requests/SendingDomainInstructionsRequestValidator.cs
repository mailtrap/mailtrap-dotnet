namespace Mailtrap.SendingDomains.Requests;


internal sealed class SendingDomainInstructionsRequestValidator : AbstractValidator<SendingDomainInstructionsRequest>
{
    public static SendingDomainInstructionsRequestValidator Instance { get; } = new();

    public SendingDomainInstructionsRequestValidator()
    {
        RuleFor(r => r.Email).NotNull().EmailAddress();
    }
}
