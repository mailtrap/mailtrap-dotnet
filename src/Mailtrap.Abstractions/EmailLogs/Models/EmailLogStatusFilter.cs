namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Filter by message status. Operators: equal, not_equal. Value(s): delivered, not_delivered, enqueued, opted_out.
/// </summary>
public sealed record EmailLogStatusFilter : IEmailLogFilterField
{
    /// <summary>Operator (equal, not_equal).</summary>
    public EmailLogFilterOperatorEqualNotEqual Operator { get; set; } = EmailLogFilterOperatorEqualNotEqual.Equal;
    /// <summary>Single status value.</summary>
    public EmailLogStatus? Value { get; set; }
    /// <summary>Multiple status values (emitted as repeated value params).</summary>
    public IList<EmailLogStatus> Values { get; } = [];

    IEnumerable<KeyValuePair<string, string>> IEmailLogFilterField.ToQueryParameters(string prefix, string field)
        => EmailLogFilterQuery.EmitWithValues(prefix, field, Operator.ToString(), Value, Values);
}
