namespace Mailtrap.Webhooks.Models;


/// <summary>
/// Webhook type. Determines which events the webhook can subscribe to.
/// </summary>
public sealed record WebhookType : StringEnum<WebhookType>
{
    /// <summary>
    /// Gets the value representing "email_sending" webhook type.
    /// </summary>
    ///
    /// <value>
    /// Represents "email_sending" webhook type.
    /// </value>
    public static readonly WebhookType EmailSending = Define("email_sending");

    /// <summary>
    /// Gets the value representing "audit_log" webhook type.
    /// </summary>
    ///
    /// <value>
    /// Represents "audit_log" webhook type.
    /// </value>
    public static readonly WebhookType AuditLog = Define("audit_log");
}
