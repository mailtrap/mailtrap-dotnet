namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Case-insensitive filter operators for to, from, and similar text fields (API: ci_contain, ci_not_contain, ci_equal, ci_not_equal).
/// </summary>
public sealed record EmailLogFilterOperatorCi : StringEnum<EmailLogFilterOperatorCi>
{
    /// <summary>Case-insensitive contain.</summary>
    public static readonly EmailLogFilterOperatorCi CiContain = Define("ci_contain");
    /// <summary>Case-insensitive not contain.</summary>
    public static readonly EmailLogFilterOperatorCi CiNotContain = Define("ci_not_contain");
    /// <summary>Case-insensitive equal.</summary>
    public static readonly EmailLogFilterOperatorCi CiEqual = Define("ci_equal");
    /// <summary>Case-insensitive not equal.</summary>
    public static readonly EmailLogFilterOperatorCi CiNotEqual = Define("ci_not_equal");
}
