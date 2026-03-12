namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Filter by string with case-insensitive operators (to, from, email_service_provider_response, recipient_mx).
/// Operators: ci_contain, ci_not_contain, ci_equal, ci_not_equal.
/// For ci_equal and ci_not_equal, either <see cref="Value"/> or <see cref="Values"/> may be set (array emits repeated value params).
/// </summary>
public sealed record EmailLogCiStringFilter : IEmailLogFilterField
{
    /// <summary>Operator (ci_contain, ci_not_contain, ci_equal, ci_not_equal).</summary>
    public EmailLogFilterOperatorCi Operator { get; set; } = EmailLogFilterOperatorCi.CiEqual;
    /// <summary>Single string value.</summary>
    public string Value { get; set; } = string.Empty;
    /// <summary>Multiple values for ci_equal / ci_not_equal (emitted as repeated filters[field][value] params).</summary>
    public IList<string> Values { get; } = [];

    IEnumerable<KeyValuePair<string, string>> IEmailLogFilterField.ToQueryParameters(string prefix, string field)
        => EmailLogFilterQuery.EmitWithValues(prefix, field, Operator.ToString(), Value, Values);
}
