namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Filter clause with operator and single value or multiple values.
/// For operators equal, not_equal, ci_equal, ci_not_equal, include_event, not_include_event the value may be single or an array.
/// </summary>
public sealed record EmailLogsFilterClause
{
    /// <summary>
    /// Filter operator (e.g. ci_equal, equal, greater_than).
    /// </summary>
    public string Operator { get; set; } = string.Empty;

    /// <summary>
    /// Single value. Use when filtering with one value.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Multiple values. When non-empty, emitted as repeated filters[field][value]=... query params.
    /// Used for equal, not_equal, ci_equal, ci_not_equal, include_event, not_include_event.
    /// </summary>
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<string> Values { get; } = [];

    /// <summary>
    /// Numeric value for number filters (e.g. clicks_count, opens_count, sending_domain_id).
    /// When set, used as the value in filters[field][value]=...
    /// </summary>
    public long? ValueLong { get; set; }
}
