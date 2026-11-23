using System.Globalization;

using Mailtrap;
using Mailtrap.EmailCampaigns;
using Mailtrap.EmailCampaigns.Models;
using Mailtrap.EmailCampaigns.Requests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


HostApplicationBuilder hostBuilder = Host.CreateApplicationBuilder(args);

hostBuilder.Services.AddMailtrapClient(hostBuilder.Configuration.GetSection("Mailtrap"));

using IHost host = hostBuilder.Build();

ILogger<Program> logger = host.Services.GetRequiredService<ILogger<Program>>();
IMailtrapClient mailtrapClient = host.Services.GetRequiredService<IMailtrapClient>();

try
{
    // Campaigns are token-scoped: the account is resolved from the API token,
    // so the resource hangs off the client root rather than off an account.
    IEmailCampaignCollectionResource campaignsResource = mailtrapClient.EmailCampaigns();

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

    // Create a new email campaign (always created in the "draft" state).
    // The request body is sent flat - no envelope.
    var createRequest = new CreateEmailCampaignRequest
    {
        Name = "Spring Sale",
        DomainId = 4321,
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
        },
        ContactListIds = [55, 56],
        ContactSegmentIds = [12]
    };
    EmailCampaign campaign = await campaignsResource.Create(createRequest);
    logger.LogInformation("Created campaign {Id} in state {State}.", campaign.Id, campaign.CurrentState);

    // Get resource for the specific campaign.
    IEmailCampaignResource campaignResource = mailtrapClient.EmailCampaign(campaign.Id);

    // Get details.
    campaign = await campaignResource.GetDetails();
    logger.LogInformation("Campaign: {Campaign}", campaign);

    // Update the draft (partial - only the provided attributes change).
    // Add the design and throttle sending to 1000 emails/hour.
    var updateRequest = new UpdateEmailCampaignRequest
    {
        Name = "Spring Sale (updated)",
        DeliveryMode = DeliveryMode.Gradual,
        DeliveryOptions = new EmailCampaignDeliveryOptions { EmailsPerHour = 1000 },
        TemplateAttributes = new EmailCampaignTemplateAttributes
        {
            Subject = "Spring is here, {{first_name}} — 30% off",
            BodyHtml = "<html><body><h1>Hi {{first_name}}!</h1>" +
                "<p><a href=\"__unsubscribe_url__\">Unsubscribe</a></p></body></html>",
            MergeTags = ["first_name"]
        }
    };
    campaign = await campaignResource.Update(updateRequest);
    logger.LogInformation("Updated campaign {Id}.", campaign.Id);

    // Schedule the campaign to start sending at a future time (must be within 1 month).
    var scheduleRequest = new ScheduleEmailCampaignRequest(DateTimeOffset.UtcNow.AddDays(7));
    campaign = await campaignResource.Schedule(scheduleRequest);
    logger.LogInformation("Campaign {Id} is {State}.", campaign.Id, campaign.CurrentState);

    // Cancel the schedule - the campaign returns to the "draft" state.
    campaign = await campaignResource.Cancel();
    logger.LogInformation("Campaign {Id} is back to {State}.", campaign.Id, campaign.CurrentState);

    // Or start sending immediately. Disabled by default so running this example
    // does not send real emails - opt in with MAILTRAP_START_CAMPAIGN=true.
    if (Environment.GetEnvironmentVariable("MAILTRAP_START_CAMPAIGN") == "true")
    {
        campaign = await campaignResource.Start();
        logger.LogInformation("Campaign {Id} is {State}.", campaign.Id, campaign.CurrentState);

        // Abort the sending before cleanup. A scheduled campaign can also be
        // reset back to draft with Reset().
        campaign = await campaignResource.Terminate();
        logger.LogInformation("Campaign {Id} is {State}.", campaign.Id, campaign.CurrentState);
    }

    // Get aggregated statistics, optionally narrowed to a date window
    // (here: the last 7 days relative to the current run).
    DateTimeOffset today = DateTimeOffset.UtcNow;
    EmailCampaignStats stats = await campaignResource.GetStats(
        new EmailCampaignStatsFilter
        {
            StartDate = FormatDate(today.AddDays(-7)),
            EndDate = FormatDate(today)
        });
    logger.LogInformation("Stats: delivered={Delivered}, opened={Opened}, delivery rate={Rate:P2}.",
        stats.DeliveryCount, stats.OpenCount, stats.DeliveryRate);

    // Only a campaign in the "draft" state can be deleted, and a started campaign never
    // returns to "draft" - so delete a fresh draft. The API returns HTTP 204 with no body.
    // Beware that the campaign resource becomes invalid after deletion and should not be used anymore.
    EmailCampaign throwaway = await campaignsResource.Create(new CreateEmailCampaignRequest
    {
        Name = "Draft to delete",
        DomainId = 4321,
        FromLocalPart = "news",
        TemplateAttributes = new EmailCampaignTemplateAttributes
        {
            Subject = "Draft to delete"
        }
    });
    await mailtrapClient.EmailCampaign(throwaway.Id).Delete();
    logger.LogInformation("Deleted campaign {Id}.", throwaway.Id);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.FailFast(ex.Message);
    throw;
}

internal sealed partial class Program
{
    // Kept outside the top-level statements to stay within the CA1506 coupling limit.
    private static string FormatDate(DateTimeOffset date) =>
        date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
}
