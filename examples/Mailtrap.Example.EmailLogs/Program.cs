using Mailtrap;
using Mailtrap.Accounts;
using Mailtrap.EmailLogs;
using Mailtrap.EmailLogs.Models;
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
    IEmailLogCollectionResource emailLogsResource = accountResource.EmailLogs();

    // List email logs with optional date filter
    var filter = new EmailLogsListFilter
    {
        SentAfter = DateTimeOffset.UtcNow.AddDays(-2),
        SentBefore = DateTimeOffset.UtcNow,
        Subject = new EmailLogCiStringOptionalFilter { Operator = EmailLogFilterOperatorCiStringOptional.NotEmpty },
        To = new EmailLogCiStringFilter { Operator = EmailLogFilterOperatorCi.CiEqual, Value = "recipient@example.com" },
        Category = new EmailLogExactStringFilter { Operator = EmailLogFilterOperatorEqualNotEqual.Equal, Values = { "Welcome Email" } }
    };

    EmailLogsListResponse response = await emailLogsResource.List(filter);

    logger.LogInformation("Total count: {TotalCount}, Messages in page: {Count}", response.TotalCount, response.Messages.Count);

    foreach (EmailLogMessage message in response.Messages)
    {
        logger.LogInformation("Message {MessageId}: {Status}, From={From}, To={To}, SentAt={SentAt}",
            message.MessageId, message.Status, message.From, message.To, message.SentAt);
    }

    // Pagination: use next_page_cursor to fetch the next page
    if (!string.IsNullOrEmpty(response.NextPageCursor))
    {
        EmailLogsListResponse nextPage = await emailLogsResource.List(filter, response.NextPageCursor);
        logger.LogInformation("Next page: {Count} messages", nextPage.Messages.Count);
    }

    // Get a single message by ID (if we have at least one)
    EmailLogMessage? firstMessage = response.Messages.FirstOrDefault();
    if (firstMessage?.MessageId is not null)
    {
        IEmailLogResource messageResource = accountResource.EmailLog(firstMessage.MessageId);
        EmailLogMessage details = await messageResource.GetDetails();
        logger.LogInformation("Message details: RawMessageUrl={HasUrl}, Events={EventCount}",
            !string.IsNullOrEmpty(details.RawMessageUrl), details.Events?.Count ?? 0);
    }
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.ExitCode = 1;
    return;
}
