namespace Mailtrap.Webhooks.Responses;


/// <summary>
/// Response object for webhook creation.
/// </summary>
///
/// <remarks>
/// Extends <see cref="Webhook"/> with <see cref="SigningSecret"/>, which is only returned upon webhook creation.
/// </remarks>
public sealed record CreateWebhookResponse : Webhook
{
    /// <summary>
    /// Gets or sets the secret key for verifying webhook payload signatures using HMAC SHA-256.
    /// </summary>
    ///
    /// <remarks>
    /// Only returned at creation time. Store it securely - it cannot be retrieved later.
    /// </remarks>
    ///
    /// <value>
    /// Signing secret.
    /// </value>
    [JsonPropertyName("signing_secret")]
    [JsonPropertyOrder(9)]
    public string SigningSecret { get; set; } = string.Empty;
}
