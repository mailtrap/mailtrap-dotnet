namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Represents the status of an email log message.
/// </summary>
public sealed record EmailLogStatus : StringEnum<EmailLogStatus>
{
    /// <summary>
    /// Message was delivered.
    /// </summary>
    public static readonly EmailLogStatus Delivered = Define("delivered");

    /// <summary>
    /// Message was not delivered.
    /// </summary>
    public static readonly EmailLogStatus NotDelivered = Define("not_delivered");

    /// <summary>
    /// Message is enqueued.
    /// </summary>
    public static readonly EmailLogStatus Enqueued = Define("enqueued");

    /// <summary>
    /// Recipient opted out.
    /// </summary>
    public static readonly EmailLogStatus OptedOut = Define("opted_out");
}
