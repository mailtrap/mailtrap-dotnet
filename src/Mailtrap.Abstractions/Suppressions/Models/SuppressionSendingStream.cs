namespace Mailtrap.Suppressions.Models;

/// <summary>
/// Suppression sending stream.
/// </summary>
public sealed record SuppressionSendingStream : StringEnum<SuppressionSendingStream>
{
    /// <summary>
    /// Gets the value representing "any" sending stream.
    /// </summary>
    ///
    /// <value>
    /// Represents "any" sending stream.
    /// </value>
    public static readonly SuppressionSendingStream Any = Define("any");

    /// <summary>
    /// Gets the value representing "transactional" sending stream.
    /// </summary>
    ///
    /// <value>
    /// Represents "transactional" sending stream.
    /// </value>
    public static readonly SuppressionSendingStream Transactional = Define("transactional");

    /// <summary>
    /// Gets the value representing "bulk" sending stream.
    /// </summary>
    ///
    /// <value>
    /// Represents "bulk" sending stream.
    /// </value>
    public static readonly SuppressionSendingStream Bulk = Define("bulk");
}
