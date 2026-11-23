namespace Mailtrap.Suppressions.Validators;


/// <summary>
/// Validator for <see cref="CreateSuppressionRequest"/>.<br/>
/// Ensures the email address, sending domain identifier and sending stream are provided,
/// and that the type is not <see cref="SuppressionType.TopicUnsubscription"/>, which the API returns but never creates.
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
            .NotEqual(SuppressionType.TopicUnsubscription)
            .WithMessage("'Type' can't be 'topic unsubscription': topic unsubscriptions are read-only.");
    }
}
