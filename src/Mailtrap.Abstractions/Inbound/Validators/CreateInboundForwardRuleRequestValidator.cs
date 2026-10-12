namespace Mailtrap.Inbound.Validators;


/// <summary>
/// Validator for <see cref="CreateInboundForwardRuleRequest"/>.
/// </summary>
public sealed class CreateInboundForwardRuleRequestValidator : AbstractValidator<CreateInboundForwardRuleRequest>
{
    /// <summary>
    /// Static validator instance for reuse.
    /// </summary>
    public static CreateInboundForwardRuleRequestValidator Instance { get; } = new();

    /// <summary>
    /// Primary constructor.
    /// </summary>
    public CreateInboundForwardRuleRequestValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty();

        RuleForEach(r => r.Conditions)
            .NotNull()
            .SetValidator(InboundForwardRuleConditionValidator.Instance);

        RuleForEach(r => r.Destinations)
            .NotNull();
    }
}
