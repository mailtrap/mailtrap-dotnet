using Mailtrap;
using Mailtrap.Accounts;
using Mailtrap.EmailCampaigns;
using Mailtrap.EmailCampaigns.Models;
using Mailtrap.EmailCampaigns.Requests;
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

    // Get resource for the email campaigns collection.
    // Note: campaigns are token-scoped - the underlying path is "/api/email_campaigns",
    // NOT "/api/accounts/{accountId}/email_campaigns".
    IEmailCampaignCollectionResource campaignsResource = accountResource.EmailCampaigns();

    // List email campaigns (paginated, newest first).
    // The name filter is sent as the "search" query parameter.
    var filter = new EmailCampaignListFilter
    {
        PerPage = 50,
        Search = "Spring",
        Token = 1
    };
    EmailCampaignList page = await campaignsResource.GetAll(filter);
    logger.LogInformation("Fetched {Count} campaign(s). Next page token: {NextToken}.",
        page.Data.Count, page.Pagination?.NextToken);

    // Create a new email campaign (created in the "draft" state).
    var createRequest = new CreateEmailCampaignRequest
    {
        Name = "Spring Sale",
        MailsendDomainId = 123,
        FromDisplayName = "Acme Marketing",
        FromLocalPart = "news",
        ReplyTo = new ReplyTo
        {
            DisplayName = "Acme Support",
            LocalPart = "support",
            Domain = "acme.com"
        },
        TemplateAttributes = new EmailCampaignTemplateAttributes
        {
            Subject = "Spring is here — 30% off"
        }
    };
    EmailCampaign campaign = await campaignsResource.Create(createRequest);
    logger.LogInformation("Created campaign {Id} in state {State}.", campaign.Id, campaign.CurrentState);

    // Get resource for the specific campaign.
    IEmailCampaignResource campaignResource = accountResource.EmailCampaign(campaign.Id);

    // Get details.
    campaign = await campaignResource.GetDetails();
    logger.LogInformation("Campaign: {Campaign}", campaign);

    // Update the campaign (partial; the campaign must not be in a sending state).
    // Switch to scheduled delivery and throttle to 1000 emails/hour.
    var updateRequest = new UpdateEmailCampaignRequest
    {
        Name = "Spring Sale (updated)",
        DeliveryMode = DeliveryMode.Scheduled,
        ScheduledFor = new DateTimeOffset(2026, 6, 1, 9, 0, 0, TimeSpan.Zero),
        DeliveryOptions = new EmailCampaignDeliveryOptions { EmailsPerHour = 1000 },
        // Pass an existing template id to update it in place instead of creating a new one.
        TemplateAttributes = new EmailCampaignTemplateAttributes
        {
            Id = campaign.Template?.Id ?? 0,
            Subject = "New subject"
        }
    };
    campaign = await campaignResource.Update(updateRequest);
    logger.LogInformation("Updated campaign {Id}; scheduled for {ScheduledFor}.",
        campaign.Id, campaign.ScheduledFor);

    // Get aggregated statistics for the campaign.
    EmailCampaignStats stats = await campaignResource.GetStats();
    logger.LogInformation("Stats: delivered={Delivered}, opened={Opened}, delivery rate={Rate:P2}.",
        stats.DeliveryCount, stats.OpenCount, stats.DeliveryRate);

    // Delete the campaign. The deleted campaign is returned (HTTP 200 + body, not 204).
    // Beware that the campaign resource becomes invalid after deletion and should not be used anymore.
    EmailCampaign deleted = await campaignResource.Delete();
    logger.LogInformation("Deleted campaign {Id}.", deleted.Id);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.FailFast(ex.Message);
    throw;
}
