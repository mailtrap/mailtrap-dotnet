namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Filter operators for status, exact, sending_domain_id, sending_stream (API: equal, not_equal).
/// </summary>
public sealed record EmailLogFilterOperatorEqualNotEqual : StringEnum<EmailLogFilterOperatorEqualNotEqual>
{
    /// <summary>Equal.</summary>
    public static readonly EmailLogFilterOperatorEqualNotEqual Equal = Define("equal");
    /// <summary>Not equal.</summary>
    public static readonly EmailLogFilterOperatorEqualNotEqual NotEqual = Define("not_equal");
}
