namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Filter operators for CI string with optional value (e.g. subject). API: ci_contain, ci_not_contain, ci_equal, ci_not_equal, empty, not_empty.
/// </summary>
public sealed record EmailLogFilterOperatorCiStringOptional : StringEnum<EmailLogFilterOperatorCiStringOptional>
{
    /// <summary>Case-insensitive contain.</summary>
    public static readonly EmailLogFilterOperatorCiStringOptional CiContain = Define("ci_contain");
    /// <summary>Case-insensitive not contain.</summary>
    public static readonly EmailLogFilterOperatorCiStringOptional CiNotContain = Define("ci_not_contain");
    /// <summary>Case-insensitive equal.</summary>
    public static readonly EmailLogFilterOperatorCiStringOptional CiEqual = Define("ci_equal");
    /// <summary>Case-insensitive not equal.</summary>
    public static readonly EmailLogFilterOperatorCiStringOptional CiNotEqual = Define("ci_not_equal");
    /// <summary>Value is empty.</summary>
    public static readonly EmailLogFilterOperatorCiStringOptional Empty = Define("empty");
    /// <summary>Value is not empty.</summary>
    public static readonly EmailLogFilterOperatorCiStringOptional NotEmpty = Define("not_empty");
}
