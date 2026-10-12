namespace Mailtrap.Inbound.Validators;


/// <summary>
/// Validator for <see cref="InboundForwardRuleCondition"/>.
/// </summary>
public sealed class InboundForwardRuleConditionValidator : AbstractValidator<InboundForwardRuleCondition>
{
    /// <summary>
    /// Static validator instance for reuse.
    /// </summary>
    public static InboundForwardRuleConditionValidator Instance { get; } = new();

    /// <summary>
    /// Primary constructor.
    /// </summary>
    public InboundForwardRuleConditionValidator()
    {
        RuleFor(c => c.MatchType)
            .Must(t => t is not null && t != ForwardRuleMatchType.None && t != ForwardRuleMatchType.Unknown)
            .WithMessage("'MatchType' must be set to a valid value.");

        RuleFor(c => c.Operator)
            .Must(o => o is not null && o != ForwardRuleOperator.None && o != ForwardRuleOperator.Unknown)
            .WithMessage("'Operator' must be set to a valid value.");
    }
}
