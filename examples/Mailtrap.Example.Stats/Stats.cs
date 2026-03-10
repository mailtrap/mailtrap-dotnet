using Mailtrap;
using Mailtrap.Stats;
using Mailtrap.Stats.Models;
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

    IStatsResource statsResource = mailtrapClient
        .Account(accountId)
        .Stats();

    var filter = new StatsFilter
    {
        StartDate = "2026-01-01",
        EndDate = "2026-01-31"
    };

    // Get aggregated stats
    SendingStats stats = await statsResource.GetStats(filter);
    logger.LogInformation("Aggregated Stats - Deliveries: {DeliveryCount}, Opens: {OpenCount}", stats.DeliveryCount, stats.OpenCount);

    // Get stats grouped by domains
    IList<SendingStatGroup> domainStats = await statsResource.ByDomain(filter);
    foreach (SendingStatGroup group in domainStats)
    {
        logger.LogInformation("Domain {Value}: Deliveries={DeliveryCount}", group.Value, group.Stats.DeliveryCount);
    }

    // Get stats grouped by categories
    IList<SendingStatGroup> categoryStats = await statsResource.ByCategory(filter);
    foreach (SendingStatGroup group in categoryStats)
    {
        logger.LogInformation("Category {Value}: Deliveries={DeliveryCount}", group.Value, group.Stats.DeliveryCount);
    }

    // Get stats grouped by email service providers
    IList<SendingStatGroup> espStats = await statsResource.ByEmailServiceProvider(filter);
    foreach (SendingStatGroup group in espStats)
    {
        logger.LogInformation("ESP {Value}: Deliveries={DeliveryCount}", group.Value, group.Stats.DeliveryCount);
    }

    // Get stats grouped by date
    IList<SendingStatGroup> dateStats = await statsResource.ByDate(filter);
    foreach (SendingStatGroup group in dateStats)
    {
        logger.LogInformation("Date {Value}: Deliveries={DeliveryCount}", group.Value, group.Stats.DeliveryCount);
    }

    // Get stats with filters
    var filteredFilter = new StatsFilter
    {
        StartDate = "2026-01-01",
        EndDate = "2026-01-31"
    };
    filteredFilter.SendingDomainIds.Add(1);
    filteredFilter.SendingDomainIds.Add(2);
    filteredFilter.SendingStreams.Add("transactional");
    filteredFilter.Categories.Add("Welcome");
    filteredFilter.EmailServiceProviders.Add("Gmail");

    SendingStats filteredStats = await statsResource.GetStats(filteredFilter);
    logger.LogInformation("Filtered Stats - Deliveries: {DeliveryCount}", filteredStats.DeliveryCount);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.ExitCode = 1;
    return;
}
