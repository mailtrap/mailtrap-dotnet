namespace Mailtrap.Webhooks.Models;


/// <summary>
/// Sending stream the webhook is subscribed to. Applicable only for <c>email_sending</c> webhooks.
/// </summary>
public sealed record WebhookSendingStream : StringEnum<WebhookSendingStream>
{
    /// <summary>
    /// Gets the value representing "transactional" sending stream.
    /// </summary>
    ///
    /// <value>
    /// Represents "transactional" sending stream.
    /// </value>
    public static readonly WebhookSendingStream Transactional = Define("transactional");

    /// <summary>
    /// Gets the value representing "bulk" sending stream.
    /// </summary>
    ///
    /// <value>
    /// Represents "bulk" sending stream.
    /// </value>
    public static readonly WebhookSendingStream Bulk = Define("bulk");
}
