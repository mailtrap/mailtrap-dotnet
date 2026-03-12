namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Filter by sending stream. Operators: equal, not_equal. Value(s): transactional, bulk.
/// Either <see cref="Value"/> or <see cref="Values"/> may be set (array emits repeated value params).
/// </summary>
public sealed record EmailLogSendingStreamFilter : IEmailLogFilterField
{
    /// <summary>Operator (equal, not_equal).</summary>
    public EmailLogFilterOperatorEqualNotEqual Operator { get; set; } = EmailLogFilterOperatorEqualNotEqual.Equal;
    /// <summary>Single sending stream (transactional, bulk).</summary>
    public SendingStream? Value { get; set; }
    /// <summary>Multiple sending streams (emitted as repeated value params).</summary>
    public IList<SendingStream> Values { get; } = [];

    IEnumerable<KeyValuePair<string, string>> IEmailLogFilterField.ToQueryParameters(string prefix, string field)
        => EmailLogFilterQuery.EmitWithValues(prefix, field, Operator.ToString(), Value, Values);
}
