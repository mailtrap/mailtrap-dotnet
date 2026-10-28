using System.Globalization;

using Mailtrap;
using Mailtrap.Marketing;
using Mailtrap.Marketing.Models;
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
    // Email Marketing stats are token-scoped: the account is resolved from the API token,
    // so the resource hangs off the client root rather than off an account.
    IMarketingStatsResource marketingStatsResource = mailtrapClient.MarketingStats();

    // The last 30 days (both start and end dates are inclusive).
    DateTime today = DateTime.UtcNow.Date;
    string startDate = today.AddDays(-29).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    string endDate = today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    // Get aggregated stats for campaigns and automations together.
    var filter = new MarketingStatsFilter
    {
        StartDate = startDate,
        EndDate = endDate
    };
    MarketingStats stats = await marketingStatsResource.GetStats(filter);
    logger.LogInformation(
        "Email Marketing - Sent: {SentCount}, Delivered: {DeliveryCount} ({DeliveryRate:P2}), Opened: {OpenCount} ({OpenRate:P2}), Clicked: {ClickCount} ({ClickRate:P2})",
        stats.SentCount, stats.DeliveryCount, stats.DeliveryRate, stats.OpenCount, stats.OpenRate, stats.ClickCount, stats.ClickRate);

    // Narrow the stats to campaigns only ("campaigns" or "automations"),
    // and to specific sending domains and email service providers.
    var campaignsFilter = new MarketingStatsFilter
    {
        StartDate = startDate,
        EndDate = endDate,
        Sources = { "campaigns" },
        // Replace with your own sending domain ID: an ID outside the account returns 400.
        SendingDomainIds = { 3938 },
        EmailServiceProviders = { "Google" }
    };
    MarketingStats campaignStats = await marketingStatsResource.GetStats(campaignsFilter);
    logger.LogInformation(
        "Campaigns - Delivered: {DeliveryCount}, Unsubscribed: {UnsubscriptionCount} ({UnsubscriptionRate:P2})",
        campaignStats.DeliveryCount, campaignStats.UnsubscriptionCount, campaignStats.UnsubscriptionRate);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.ExitCode = 1;
    return;
}
