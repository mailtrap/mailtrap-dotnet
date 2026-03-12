namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Filter by sending domain ID. Operators: equal, not_equal.
/// Either <see cref="Value"/> or <see cref="Values"/> may be set (array emits repeated value params).
/// </summary>
public sealed record EmailLogSendingDomainIdFilter : IEmailLogFilterField
{
    /// <summary>Operator (equal, not_equal).</summary>
    public EmailLogFilterOperatorEqualNotEqual Operator { get; set; } = EmailLogFilterOperatorEqualNotEqual.Equal;
    /// <summary>Single sending domain ID.</summary>
    public long? Value { get; set; }
    /// <summary>Multiple sending domain IDs (emitted as repeated filters[field][value] params).</summary>
    public IList<long> Values { get; } = [];

    IEnumerable<KeyValuePair<string, string>> IEmailLogFilterField.ToQueryParameters(string prefix, string field)
        => EmailLogFilterQuery.EmitWithValues(prefix, field, Operator.ToString(), Value, Values);
}
