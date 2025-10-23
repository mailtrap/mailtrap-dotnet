namespace Mailtrap.Emails;


/// <summary>
/// Base Mailtrap API client for sending emails.
/// </summary>
/// <typeparam name="TRequest">
/// The type of the email request object.
/// One of <see cref="SendEmailRequest"/> or <see cref="BatchEmailRequest"/>.
/// </typeparam>
/// <typeparam name="TResponse">
/// The type of the email response object.
/// One of <see cref="SendEmailResponse"/> or <see cref="BatchEmailResponse"/>.
/// </typeparam>
public interface IEmailClient<TRequest, TResponse> : IRestResource
    where TRequest : class
    where TResponse : class
{
    /// <summary>
    /// Sends email, represented by the <paramref name="request"/>, and returns send operation result.
    /// </summary>
    ///
    /// <param name="request">
    /// Request object, containing email data.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// <typeparamref name="TResponse"/> instance with response data.
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> is <see langword="null"/>.
    /// </exception>
    ///
    /// <exception cref="ArgumentException">
    /// When <paramref name="request"/> contains invalid data.
    /// </exception>
    ///
    /// <exception cref="JsonException">
    /// When request serialization or API response deserialization fails for any reason.
    /// </exception>
    ///
    /// <exception cref="TaskCanceledException">
    /// When operation is canceled by <paramref name="cancellationToken"/>.
    /// </exception>
    ///
    /// <exception cref="OperationCanceledException">
    /// When operation is canceled by <paramref name="cancellationToken"/>.
    /// </exception>
    ///
    /// <exception cref="HttpRequestException">
    /// When request to the API fails for any reason.
    /// </exception>
    ///
    /// <exception cref="MailtrapApiException">
    /// When request failed for any other reason.
    /// </exception>
    ///
    /// <remarks>
    /// Request is checked for validity before send.<br />
    /// <see cref="ArgumentException"/> is thrown if validation fails.
    /// </remarks>
    public Task<TResponse> Send(TRequest request, CancellationToken cancellationToken = default);
}
