namespace Mailtrap.Core.Models;


/// <summary>
/// Represents an email sending stream.
/// </summary>
public sealed record SendingStream : StringEnum<SendingStream>
{
    /// <summary>
    /// Transactional sending stream.
    /// </summary>
    public static readonly SendingStream Transactional = Define("transactional");

    /// <summary>
    /// Bulk sending stream.
    /// </summary>
    public static readonly SendingStream Bulk = Define("bulk");
}
