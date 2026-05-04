namespace Mailtrap.Webhooks.Requests;


internal static class WebhookRequestExtensions
{
    public static CreateWebhookRequestDto ToDto(this CreateWebhookRequest request) => new(request);

    public static UpdateWebhookRequestDto ToDto(this UpdateWebhookRequest request) => new(request);
}
