namespace Mailtrap.Inbound.Models;


/// <summary>
/// Comparison operator of a forward rule condition.
/// </summary>
public sealed record ForwardRuleOperator : StringEnum<ForwardRuleOperator>
{
    /// <summary>
    /// Value equals the condition value.
    /// </summary>
    public static readonly ForwardRuleOperator Equal = Define("equal");

    /// <summary>
    /// Value does not equal the condition value.
    /// </summary>
    public static readonly ForwardRuleOperator NotEqual = Define("not_equal");

    /// <summary>
    /// Value contains the condition value.
    /// </summary>
    public static readonly ForwardRuleOperator Contains = Define("contains");

    /// <summary>
    /// Value starts with the condition value.
    /// </summary>
    public static readonly ForwardRuleOperator StartsWith = Define("starts_with");

    /// <summary>
    /// Value ends with the condition value.
    /// </summary>
    public static readonly ForwardRuleOperator EndsWith = Define("ends_with");

    /// <summary>
    /// Value is empty.
    /// </summary>
    public static readonly ForwardRuleOperator Empty = Define("empty");

    /// <summary>
    /// Value is not empty.
    /// </summary>
    public static readonly ForwardRuleOperator NotEmpty = Define("not_empty");
}
