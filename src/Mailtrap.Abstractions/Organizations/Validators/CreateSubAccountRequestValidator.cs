namespace Mailtrap.Organizations.Validators;


/// <summary>
/// Validator for <see cref="CreateSubAccountRequest"/>.
/// </summary>
public sealed class CreateSubAccountRequestValidator : AbstractValidator<CreateSubAccountRequest>
{
    /// <summary>
    /// Static validator instance for reuse.
    /// </summary>
    public static CreateSubAccountRequestValidator Instance { get; } = new();

    /// <summary>
    /// Primary constructor.
    /// </summary>
    public CreateSubAccountRequestValidator()
    {
        RuleFor(r => r.Account).NotNull();
        RuleFor(r => r.Account.Name).NotEmpty().MaximumLength(255);
    }
}
