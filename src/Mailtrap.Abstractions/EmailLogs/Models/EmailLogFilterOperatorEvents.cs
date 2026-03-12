namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Filter operators for events (API: include_event, not_include_event).
/// </summary>
public sealed record EmailLogFilterOperatorEvents : StringEnum<EmailLogFilterOperatorEvents>
{
    /// <summary>Include messages that have this event.</summary>
    public static readonly EmailLogFilterOperatorEvents IncludeEvent = Define("include_event");
    /// <summary>Exclude messages that have this event.</summary>
    public static readonly EmailLogFilterOperatorEvents NotIncludeEvent = Define("not_include_event");
}
