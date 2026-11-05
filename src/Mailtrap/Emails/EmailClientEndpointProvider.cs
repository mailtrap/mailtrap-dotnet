namespace Mailtrap.Emails;


/// <summary>
/// <see cref="IEmailClientEndpointProvider"/> implementation.
/// </summary>
internal sealed class EmailClientEndpointProvider : IEmailClientEndpointProvider
{
    private const string SendEmailSegment = "send";
    private const string BatchEmailSegment = "batch";


    /// <summary>
    /// Builds the request <see cref="Uri"/> for send or batch operations.
    /// </summary>
    /// <param name="isBatch">When <c>true</c>, append the batch segment; otherwise append the send segment.</param>
    /// <param name="isBulk">When <c>true</c>, use the bulk host; otherwise use the standard send host.</param>
    /// <param name="sandboxId">When supplied, scope the request to the sandbox and switch to the sandbox host.</param>
    /// <returns>The fully qualified endpoint URI.</returns>
    public Uri GetRequestUri(bool isBatch, bool isBulk, long? sandboxId)
    {
        var rootUrl = sandboxId switch
        {
            null => isBulk ? Endpoints.BulkDefaultUrl : Endpoints.SendDefaultUrl,
            _ => Endpoints.SandboxDefaultUrl,
        };

        var emailSegment = isBatch ? BatchEmailSegment : SendEmailSegment;

        var result = rootUrl.Append(UrlSegments.ApiRootSegment, emailSegment);

        return sandboxId is null ? result : result.Append(sandboxId.Value);
    }
}
