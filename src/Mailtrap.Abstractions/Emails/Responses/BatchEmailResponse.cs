namespace Mailtrap.Emails.Responses;


/// <summary>
/// Represents batch send email response object.
/// </summary>
public sealed record BatchEmailResponse
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
    public bool Success { get; private set; } = false;

    /// <summary>
    /// Gets a collection of individual message responses that have been sent.
    /// </summary>
    ///
    /// <value>
    /// A collection of individual message responses that have been sent.
    /// </value>
    ///
    /// <remarks>
    /// The order of results is the same as the original messages.
    /// </remarks>
    [JsonPropertyName("responses")]
    [JsonPropertyOrder(2)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<BatchSendEmailResponse> Responses { get; private set; } = [];

    /// <summary>
    /// Gets errors, associated with the response.
    /// </summary>
    ///
    /// <value>
    /// Collection of error(s) details.
    /// </value>
    [JsonPropertyName("errors")]
    [JsonPropertyOrder(3)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<string> Errors { get; private set; } = [];


    internal static BatchEmailResponse CreateSuccess(params BatchSendEmailResponse[] responses)
    {
        return new BatchEmailResponse
        {
            Success = true,
            Responses = responses
        };
    }

    internal static BatchEmailResponse CreateFailure(params string[] errors)
    {
        return new BatchEmailResponse
        {
            Success = false,
            Errors = errors
        };
    }
}
