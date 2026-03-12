namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Filter by events. Operators: include_event, not_include_event. Value(s): delivery, open, click, bounce, spam, unsubscribe, soft_bounce, reject, suspension.
/// </summary>
public sealed record EmailLogEventsFilter : IEmailLogFilterField
{
    /// <summary>Operator (include_event, not_include_event).</summary>
    public EmailLogFilterOperatorEvents Operator { get; set; } = EmailLogFilterOperatorEvents.IncludeEvent;
    /// <summary>Single event type.</summary>
    public EmailLogEventType? Value { get; set; }
    /// <summary>Multiple event types (emitted as repeated value params).</summary>
    public IList<EmailLogEventType> Values { get; } = [];

    IEnumerable<KeyValuePair<string, string>> IEmailLogFilterField.ToQueryParameters(string prefix, string field)
        => EmailLogFilterQuery.EmitWithValues(prefix, field, Operator.ToString(), Value, Values);
}
