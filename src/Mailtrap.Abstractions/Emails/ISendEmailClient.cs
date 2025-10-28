namespace Mailtrap.Emails;


/// <summary>
/// Mailtrap API client for sending emails.
/// </summary>
public interface ISendEmailClient : IEmailClient<SendEmailRequest, SendEmailResponse> { }
