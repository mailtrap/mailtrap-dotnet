using System.Globalization;


namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Filter by numeric value (e.g. clicks_count, opens_count). Operators: equal, greater_than, less_than.
/// </summary>
public sealed record EmailLogNumberFilter : IEmailLogFilterField
{
    /// <summary>Operator (equal, greater_than, less_than).</summary>
    public EmailLogFilterOperatorNumber Operator { get; set; } = EmailLogFilterOperatorNumber.Equal;
    /// <summary>Numeric value.</summary>
    public long Value { get; set; }

    IEnumerable<KeyValuePair<string, string>> IEmailLogFilterField.ToQueryParameters(string prefix, string field)
    {
        return EmailLogFilterQuery.Emit(prefix, field, Operator.ToString(), new[] { Value.ToString(CultureInfo.InvariantCulture) });
    }
}
