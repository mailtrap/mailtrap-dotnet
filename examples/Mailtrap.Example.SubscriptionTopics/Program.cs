using System.Net;

using Mailtrap;
using Mailtrap.Core.Exceptions;
using Mailtrap.SubscriptionTopics;
using Mailtrap.SubscriptionTopics.Models;
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
    // Subscription topics are token-scoped: the account is resolved from the API token,
    // so the resource hangs off the client root rather than off an account.
    // They are read-only in the API: create and edit them in the Mailtrap app.
    ISubscriptionTopicCollectionResource topicsResource = mailtrapClient.SubscriptionTopics();

    // List the account's topics (paginated by id, deleted topics left out).
    SubscriptionTopicList page = await topicsResource.GetAll(new SubscriptionTopicListFilter { PerPage = 50 });
    logger.LogInformation("Fetched {Count} subscription topic(s). Next page token: {NextToken}.",
        page.Data.Count, page.Pagination?.NextToken);

    foreach (SubscriptionTopic topic in page.Data)
    {
        // A topic without a domain covers every sending domain of the account.
        if (topic.DomainId is null)
        {
            logger.LogInformation("Topic {Id} '{Name}' covers every sending domain.", topic.Id, topic.Name);
        }
        else
        {
            logger.LogInformation("Topic {Id} '{Name}' covers sending domain {DomainId}.", topic.Id, topic.Name, topic.DomainId);
        }
    }

    SubscriptionTopic? firstTopic = page.Data.FirstOrDefault();

    if (firstTopic is null)
    {
        logger.LogWarning("The account has no subscription topics yet: create one in the Mailtrap app.");
        return;
    }

    // Get a single topic. A deleted topic is returned too, with DeletedAt set,
    // since a campaign keeps a topic deleted after it was assigned.
    SubscriptionTopic details = await mailtrapClient.SubscriptionTopic(firstTopic.Id).GetDetails();
    logger.LogInformation("Topic: {Topic}", details);
}
catch (HttpRequestFailedException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
{
    logger.LogWarning("Subscription topics are not available for this account or API token.");
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.ExitCode = 1;
    return;
}
