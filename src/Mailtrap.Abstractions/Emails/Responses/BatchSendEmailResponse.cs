namespace Mailtrap.Emails.Responses;


/// <summary>
/// Represents send email response details object for batch requests.
/// </summary>
public sealed record BatchSendEmailResponse : SendEmailResponse
{
    /// <summary>
    /// Gets the errors, associated with the response.
    /// </summary>
    ///
    /// <value>
    /// Collection of error(s) details.
    /// </value>
    [JsonPropertyName("errors")]
    [JsonPropertyOrder(3)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<string> Errors { get; private set; } = [];


    internal static new BatchSendEmailResponse CreateSuccess(params string[] messageIds)
    {
        var response = new BatchSendEmailResponse
        {
            Success = true,
        };
        messageIds.ToList().ForEach(response.MessageIds.Add);

        return response;
    }

    internal static BatchSendEmailResponse CreateFailure(params string[] errors)
    {
        return new BatchSendEmailResponse
        {
            Success = false,
            Errors = errors
        };
    }

    internal static BatchSendEmailResponse CreateFailure(string[] messageIds, string[] errors)
    {
        var response = new BatchSendEmailResponse
        {
            Success = false,
            Errors = errors
        };
        messageIds.ToList().ForEach(response.MessageIds.Add);
        return response;
    }
}
