namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Filter by string (e.g. client_ip or sending_ip). Operators: equal, not_equal, contain, not_contain.
/// For equal and not_equal, either <see cref="Value"/> or <see cref="Values"/> may be set (array emits repeated value params).
/// </summary>
public sealed record EmailLogStringFilter : IEmailLogFilterField
{
    /// <summary>Operator (equal, not_equal, contain, not_contain).</summary>
    public EmailLogFilterOperatorIp Operator { get; set; } = EmailLogFilterOperatorIp.Equal;
    /// <summary>Single string value.</summary>
    public string Value { get; set; } = string.Empty;
    /// <summary>Multiple values for equal / not_equal (emitted as repeated filters[field][value] params).</summary>
    public IList<string> Values { get; } = [];

    IEnumerable<KeyValuePair<string, string>> IEmailLogFilterField.ToQueryParameters(string prefix, string field)
        => EmailLogFilterQuery.EmitWithValues(prefix, field, Operator.ToString(), Value, Values);
}
