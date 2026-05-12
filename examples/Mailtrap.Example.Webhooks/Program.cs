using Mailtrap;
using Mailtrap.Accounts;
using Mailtrap.Webhooks;
using Mailtrap.Core.Models;
using Mailtrap.Webhooks.Models;
using Mailtrap.Webhooks.Requests;
using Mailtrap.Webhooks.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


HostApplicationBuilder hostBuilder = Host.CreateApplicationBuilder(args);

IConfigurationSection config = hostBuilder.Configuration.GetSection("Mailtrap");

hostBuilder.Services.AddMailtrapClient(config);

using IHost host = hostBuilder.Build();

ILogger<Program> logger = host.Services.GetRequiredService<ILogger<Program>>();
IMailtrapClient mailtrapClient = host.Services.GetRequiredService<IMailtrapClient>();

try
{
    var accountId = 12345;
    IAccountResource accountResource = mailtrapClient.Account(accountId);

    // Get resource for webhooks collection
    IWebhookCollectionResource webhooksResource = accountResource.Webhooks();

    // List all webhooks for the account
    IList<Webhook> webhooks = await webhooksResource.GetAll();
    logger.LogInformation("Found {Count} webhook(s).", webhooks.Count);

    // Create a new "email_sending" webhook subscribed to delivery + bounce events on the transactional stream
    var createRequest = new CreateWebhookRequest
    {
        Url = new Uri("https://example.com/webhooks/mailtrap"),
        WebhookType = WebhookType.EmailSending,
        Active = true,
        PayloadFormat = WebhookPayloadFormat.Json,
        SendingStream = SendingStream.Transactional
    };
    createRequest.EventTypes.Add(WebhookEventType.Delivery);
    createRequest.EventTypes.Add(WebhookEventType.Bounce);

    CreateWebhookResponse createdWebhook = await webhooksResource.Create(createRequest);

    // The signing secret is only returned at creation time - store it securely
    logger.LogInformation(
        "Created Webhook: Id={Id}, Url={Url}, Type={Type}",
        createdWebhook.Id,
        createdWebhook.Url,
        createdWebhook.WebhookType);
    logger.LogInformation("Signing secret (store securely, returned only once): {Secret}", createdWebhook.SigningSecret);

    // Get resource for the specific webhook
    IWebhookResource webhookResource = accountResource.Webhook(createdWebhook.Id);

    // Get details of the webhook
    Webhook webhookDetails = await webhookResource.GetDetails();
    logger.LogInformation("Webhook details: Id={Id}, Url={Url}, Active={Active}",
        webhookDetails.Id,
        webhookDetails.Url,
        webhookDetails.Active);

    // Update the webhook - swap the event types and disable it
    var updateRequest = new UpdateWebhookRequest
    {
        Active = false,
        EventTypes = [WebhookEventType.Delivery, WebhookEventType.Open, WebhookEventType.Click]
    };
    Webhook updatedWebhook = await webhookResource.Update(updateRequest);
    logger.LogInformation(
        "Updated Webhook: Id={Id}, Active={Active}, EventTypes={EventTypes}",
        updatedWebhook.Id,
        updatedWebhook.Active,
        string.Join(",", updatedWebhook.EventTypes));

    // Delete the webhook
    // The webhook resource becomes invalid after deletion and should not be used anymore
    Webhook deletedWebhook = await webhookResource.Delete();
    logger.LogInformation("Webhook Deleted: Id={Id}", deletedWebhook.Id);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.ExitCode = 1;
    return;
}
