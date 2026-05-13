namespace Mailtrap.Webhooks.Models;


/// <summary>
/// Format of the webhook payload.
/// </summary>
public sealed record WebhookPayloadFormat : StringEnum<WebhookPayloadFormat>
{
    /// <summary>
    /// Gets the value representing "json" payload format.
    /// </summary>
    ///
    /// <value>
    /// Represents "json" payload format.
    /// </value>
    public static readonly WebhookPayloadFormat Json = Define("json");

    /// <summary>
    /// Gets the value representing "jsonlines" payload format.
    /// </summary>
    ///
    /// <value>
    /// Represents "jsonlines" payload format.
    /// </value>
    public static readonly WebhookPayloadFormat JsonLines = Define("jsonlines");
}
