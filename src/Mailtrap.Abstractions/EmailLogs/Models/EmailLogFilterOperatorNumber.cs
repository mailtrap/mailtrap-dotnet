namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Filter operators for numeric fields clicks_count, opens_count (API: equal, greater_than, less_than).
/// </summary>
public sealed record EmailLogFilterOperatorNumber : StringEnum<EmailLogFilterOperatorNumber>
{
    /// <summary>Equal.</summary>
    public static readonly EmailLogFilterOperatorNumber Equal = Define("equal");
    /// <summary>Greater than.</summary>
    public static readonly EmailLogFilterOperatorNumber GreaterThan = Define("greater_than");
    /// <summary>Less than.</summary>
    public static readonly EmailLogFilterOperatorNumber LessThan = Define("less_than");
}
