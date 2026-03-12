namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Represents the sending stream for an email log message.
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
