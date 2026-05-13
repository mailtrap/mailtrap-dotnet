namespace Mailtrap.ApiTokens.Validators;


/// <summary>
/// Validator for <see cref="CreateApiTokenRequest"/> requests.<br/>
/// Ensures name is not empty and that every nested resource access entry is valid.
/// </summary>
public sealed class CreateApiTokenRequestValidator : AbstractValidator<CreateApiTokenRequest>
{
    /// <summary>
    /// Static validator instance for reuse.
    /// </summary>
    public static CreateApiTokenRequestValidator Instance { get; } = new();

    /// <summary>
    /// Primary constructor.
    /// </summary>
    public CreateApiTokenRequestValidator()
    {
        RuleFor(r => r.Name).NotEmpty().MaximumLength(255);

        RuleForEach(r => r.Resources)
            .SetValidator(ApiTokenAccessRequestValidator.Instance);
    }
}
