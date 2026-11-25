using Mailtrap.Webhooks.Models;
using Mailtrap.Webhooks.Requests;
using Mailtrap.Webhooks.Validators;


namespace Mailtrap.UnitTests.Webhooks;


[TestFixture]
internal sealed class CreateWebhookRequestValidatorTests
{
    private static readonly CreateWebhookRequestValidator s_validator = CreateWebhookRequestValidator.Instance;


    [Test]
    public void EmailSendingWebhook_WithRequiredFields_ShouldPass()
    {
        var request = Request(WebhookType.EmailSending, SendingStream.Transactional);
        request.EventTypes.Add(WebhookEventType.Delivery);

        var result = s_validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void EmailSendingWebhook_WithoutEventTypes_ShouldNameEmailSending()
    {
        var request = Request(WebhookType.EmailSending, SendingStream.Transactional);

        var result = s_validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.EventTypes)
            .WithErrorMessage("'EventTypes' must contain at least one event for 'email_sending' webhook type.");
    }

    [Test]
    public void EmailSendingWebhook_WithoutSendingStream_ShouldFail()
    {
        var request = Request(WebhookType.EmailSending);
        request.EventTypes.Add(WebhookEventType.Delivery);

        var result = s_validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.SendingStream)
            .WithErrorMessage("'SendingStream' is required for 'email_sending' webhook type.");
    }

    [Test]
    public void CampaignsWebhook_WithEventTypes_ShouldPass()
    {
        var request = Request(WebhookType.Campaigns);
        request.EventTypes.Add(WebhookEventType.Delivery);

        var result = s_validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void CampaignsWebhook_WithoutEventTypes_ShouldNameCampaigns()
    {
        var result = s_validator.TestValidate(Request(WebhookType.Campaigns));

        result.ShouldHaveValidationErrorFor(r => r.EventTypes)
            .WithErrorMessage("'EventTypes' must contain at least one event for 'campaigns' webhook type.");
    }

    [Test]
    public void CampaignsWebhook_WithSendingStream_ShouldFail()
    {
        var request = Request(WebhookType.Campaigns, SendingStream.Bulk);
        request.EventTypes.Add(WebhookEventType.Delivery);

        var result = s_validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.SendingStream);
    }


    private static CreateWebhookRequest Request(WebhookType webhookType, SendingStream? sendingStream = null) => new()
    {
        Url = new Uri("https://example.com/hook"),
        WebhookType = webhookType,
        SendingStream = sendingStream
    };
}
