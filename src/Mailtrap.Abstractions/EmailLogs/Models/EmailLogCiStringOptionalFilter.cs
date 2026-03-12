namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Filter by string with case-insensitive operators and optional value (e.g. subject). Operators: ci_contain, ci_not_contain, ci_equal, ci_not_equal, empty, not_empty. Value optional for empty/not_empty.
/// For ci_equal and ci_not_equal, either <see cref="Value"/> or <see cref="Values"/> may be set (array emits repeated value params).
/// </summary>
public sealed record EmailLogCiStringOptionalFilter : IEmailLogFilterField
{
    /// <summary>Operator (value optional for empty/not_empty).</summary>
    public EmailLogFilterOperatorCiStringOptional Operator { get; set; } = EmailLogFilterOperatorCiStringOptional.CiContain;
    /// <summary>Single string value; omit for empty/not_empty.</summary>
    public string? Value { get; set; }
    /// <summary>Multiple values for ci_equal / ci_not_equal (emitted as repeated filters[field][value] params).</summary>
    public IList<string> Values { get; } = [];

    IEnumerable<KeyValuePair<string, string>> IEmailLogFilterField.ToQueryParameters(string prefix, string field)
        => EmailLogFilterQuery.EmitWithValues(prefix, field, Operator.ToString(), Value, Values);
}
