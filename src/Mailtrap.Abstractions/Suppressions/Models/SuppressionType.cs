namespace Mailtrap.Suppressions.Models;

/// <summary>
/// Suppression type.
/// </summary>
public sealed record SuppressionType : StringEnum<SuppressionType>
{
    /// <summary>
    /// Gets the value representing "hard bounce" suppression type.
    /// </summary>
    ///
    /// <value>
    /// Represents "hard bounce" suppression type.
    /// </value>
    public static readonly SuppressionType HardBounce = Define("hard bounce");

    /// <summary>
    /// Gets the value representing "spam complaint" suppression type.
    /// </summary>
    ///
    /// <value>
    /// Represents "spam complaint" suppression type.
    /// </value>
    public static readonly SuppressionType SpamComplaint = Define("spam complaint");

    /// <summary>
    /// Gets the value representing "unsubscription" suppression type.
    /// </summary>
    ///
    /// <value>
    /// Represents "unsubscription" suppression type.
    /// </value>
    public static readonly SuppressionType Unsubscription = Define("unsubscription");

    /// <summary>
    /// Gets the value representing "manual import" suppression type.
    /// </summary>
    ///
    /// <value>
    /// Represents "manual import" suppression type.
    /// </value>
    public static readonly SuppressionType ManualImport = Define("manual import");
}
