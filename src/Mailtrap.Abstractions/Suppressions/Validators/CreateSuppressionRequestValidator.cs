namespace Mailtrap.Suppressions.Validators;


/// <summary>
/// Validator for <see cref="CreateSuppressionRequest"/>.<br/>
/// Ensures the email address, sending domain identifier and sending stream are provided,
/// and that a provided type is one the API creates: <see cref="SuppressionType.TopicUnsubscription"/> is read-only.
/// </summary>
public sealed class CreateSuppressionRequestValidator : AbstractValidator<CreateSuppressionRequest>
{
    /// <summary>
    /// Static validator instance for reuse.
    /// </summary>
    public static CreateSuppressionRequestValidator Instance { get; } = new();

    /// <summary>
    /// Primary constructor.
    /// </summary>
    public CreateSuppressionRequestValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage("'Email' must not be empty.");

        RuleFor(r => r.DomainId)
            .GreaterThan(0)
            .WithMessage("'DomainId' must be a positive sending domain ID.");

        RuleFor(r => r.SendingStream)
            .NotEqual(SendingStream.Unknown)
            .WithMessage("'SendingStream' must be either transactional or bulk.");

        RuleFor(r => r.Type)
            .Must(t => t is null
                || t == SuppressionType.HardBounce
                || t == SuppressionType.SpamComplaint
                || t == SuppressionType.Unsubscription
                || t == SuppressionType.ManualImport)
            .WithMessage("'Type' must be hard bounce, spam complaint, unsubscription or manual import.");
    }
}
