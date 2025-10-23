namespace Mailtrap.Emails;


/// <summary>
/// Specialized email client for batch email operations.
/// Inherits from <see cref="IEmailClient{TRequest,TResponse}"/> with batch-specific request and response types.
/// </summary>
public interface IBatchEmailClient : IEmailClient<BatchEmailRequest, BatchEmailResponse> { }
