namespace Mailtrap.Emails;


/// <summary>
/// <see cref="IEmailClient{TRequest, TResponse}"/> generic implementation.
/// </summary>
internal class EmailClient<TRequest, TResponse> : RestResource, IEmailClient<TRequest, TResponse>
    where TRequest : class
    where TResponse : class
{
    /// <summary>
    /// Default instance constructor.
    /// </summary>
    ///
    /// <exception cref="ArgumentNullException">
    /// When any of the parameters provided is <see langword="null"/>.
    /// </exception>
    public EmailClient(IRestResourceCommandFactory restResourceCommandFactory, Uri sendUri)
        : base(restResourceCommandFactory, sendUri) { }


    /// <inheritdoc/>
    public async Task<TResponse> Send(TRequest request, CancellationToken cancellationToken = default)
        => await Create<TRequest, TResponse>(request, cancellationToken).ConfigureAwait(false);
}
