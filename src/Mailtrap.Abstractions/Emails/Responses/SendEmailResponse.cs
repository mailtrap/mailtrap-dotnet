namespace Mailtrap.Emails.Responses;


/// <summary>
/// Represents send email response object.
/// </summary>
public record SendEmailResponse
{
    /// <summary>
    /// Gets a flag, indicating whether request succeeded or failed and response contains error(s).
    /// </summary>
    ///
    /// <value>
    /// <see langword="false"/> when request failed and response contains error(s).<br/>
    /// <see langword="true"/> when request succeeded.
    /// </value>
    [JsonPropertyName("success")]
    [JsonPropertyOrder(1)]
    [JsonInclude]
    public bool Success { get; protected set; } = false;

    /// <summary>
    /// Gets a collection of IDs of emails that have been sent.
    /// </summary>
    ///
    /// <value>
    /// A collection of IDs of emails that have been sent.
    /// </value>
    [JsonPropertyName("message_ids")]
    [JsonPropertyOrder(2)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<string> MessageIds { get; private set; } = [];


    internal static SendEmailResponse CreateSuccess(params string[] messageIds)
    {
        return new SendEmailResponse
        {
            Success = true,
            MessageIds = messageIds
        };
    }
}
