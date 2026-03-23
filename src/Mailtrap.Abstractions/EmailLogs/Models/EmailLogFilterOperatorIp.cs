namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Filter operators for client_ip, sending_ip (API: equal, not_equal, contain, not_contain).
/// </summary>
public sealed record EmailLogFilterOperatorIp : StringEnum<EmailLogFilterOperatorIp>
{
    /// <summary>Equal.</summary>
    public static readonly EmailLogFilterOperatorIp Equal = Define("equal");
    /// <summary>Not equal.</summary>
    public static readonly EmailLogFilterOperatorIp NotEqual = Define("not_equal");
    /// <summary>Contain.</summary>
    public static readonly EmailLogFilterOperatorIp Contain = Define("contain");
    /// <summary>Not contain.</summary>
    public static readonly EmailLogFilterOperatorIp NotContain = Define("not_contain");
}
