namespace Mailtrap.Emails.Extensions;

/// <summary>
/// Provides extension methods for <see cref="BatchEmailRequest"/>.
/// </summary>
internal static class BatchEmailRequestExtensions
{
    /// <summary>
    /// Gets merged requests by combining each request with the Base request.
    /// </summary>
    /// <param name="batchRequest">
    /// Batch email request which contains the base request and individual email requests.
    /// </param>
    /// <returns>Collection of merged email requests.</returns>
    internal static IEnumerable<SendEmailRequest>? GetMergedRequests(this BatchEmailRequest batchRequest)
    {
        return batchRequest.Requests?.Select(request => MergeWithBase(request, batchRequest.Base));
    }

    /// <summary>
    /// Merges an individual email request with the base email request.
    /// If a property is set in the individual request, it takes precedence over the base request.
    /// If a property is not set in the individual request, it inherits the value from the base request.
    /// </summary>
    /// <remarks>
    /// Takes care of properties present only in <see cref="EmailRequest"/>.
    /// </remarks>
    /// <param name="request">Individual email request to merge.</param>
    /// <param name="baseRequest">Base email request to merge with. Can be <c>null</c>.</param>
    /// <returns>New instance of <see cref="SendEmailRequest"/> is returned.</returns>
    internal static SendEmailRequest MergeWithBase(SendEmailRequest request, EmailRequest? baseRequest)
    {
        if (request is null)
        {
            return null!;
        }

        return baseRequest is null
            ? request
            : request with
            {
                From = request.From ?? baseRequest.From,
                ReplyTo = request.ReplyTo ?? baseRequest.ReplyTo,
                Subject = string.IsNullOrEmpty(request.Subject) ? baseRequest.Subject : request.Subject,
                TextBody = string.IsNullOrEmpty(request.TextBody) ? baseRequest.TextBody : request.TextBody,
                HtmlBody = string.IsNullOrEmpty(request.HtmlBody) ? baseRequest.HtmlBody : request.HtmlBody,
                Attachments = request.Attachments ?? baseRequest.Attachments,
                Headers = request.Headers ?? baseRequest.Headers,
                Category = string.IsNullOrEmpty(request.Category) ? baseRequest.Category : request.Category,
                CustomVariables = request.CustomVariables ?? baseRequest.CustomVariables,
                TemplateId = string.IsNullOrEmpty(request.TemplateId) ? baseRequest.TemplateId : request.TemplateId,
                TemplateVariables = request.TemplateVariables ?? baseRequest.TemplateVariables
            };
    }
}
