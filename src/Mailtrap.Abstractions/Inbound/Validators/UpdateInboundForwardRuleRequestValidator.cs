namespace Mailtrap.Inbound.Validators;


/// <summary>
/// Validator for <see cref="UpdateInboundForwardRuleRequest"/>.
/// </summary>
public sealed class UpdateInboundForwardRuleRequestValidator : AbstractValidator<UpdateInboundForwardRuleRequest>
{
    /// <summary>
    /// Static validator instance for reuse.
    /// </summary>
    public static UpdateInboundForwardRuleRequestValidator Instance { get; } = new();

    /// <summary>
    /// Primary constructor.
    /// </summary>
    public UpdateInboundForwardRuleRequestValidator()
    {
        When(r => r.Name is not null, () =>
        {
            RuleFor(r => r.Name)
                .NotEmpty();
        });

        RuleForEach(r => r.Conditions)
            .NotNull()
            .SetValidator(InboundForwardRuleConditionValidator.Instance);

        RuleForEach(r => r.Destinations)
            .NotNull();
    }
}
