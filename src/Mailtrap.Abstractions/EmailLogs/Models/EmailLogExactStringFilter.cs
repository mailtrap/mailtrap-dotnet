namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Filter by exact string (email_service_provider, category). Operators: equal, not_equal.
/// Either <see cref="Value"/> or <see cref="Values"/> may be set (array emits repeated value params).
/// </summary>
public sealed record EmailLogExactStringFilter : IEmailLogFilterField
{
    /// <summary>Operator (equal, not_equal).</summary>
    public EmailLogFilterOperatorEqualNotEqual Operator { get; set; } = EmailLogFilterOperatorEqualNotEqual.Equal;
    /// <summary>Single exact string value.</summary>
    public string Value { get; set; } = string.Empty;
    /// <summary>Multiple values (emitted as repeated filters[field][value] params).</summary>
    public IList<string> Values { get; } = [];

    IEnumerable<KeyValuePair<string, string>> IEmailLogFilterField.ToQueryParameters(string prefix, string field)
        => EmailLogFilterQuery.EmitWithValues(prefix, field, Operator.ToString(), Value, Values);
}
