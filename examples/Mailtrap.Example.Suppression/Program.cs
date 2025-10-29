using Mailtrap;
using Mailtrap.Accounts;
using Mailtrap.Suppressions;
using Mailtrap.Suppressions.Models;
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

    // Get resource for suppressions collection
    ISuppressionCollectionResource suppressionsResource = accountResource.Suppressions();

    var suppressionFilter = new SuppressionFilter
    {
        Email = "test@demomailtrap.co"
    };

    IList<Suppression> suppressions = await suppressionsResource.Fetch(suppressionFilter);

    Suppression? suppression = suppressions.FirstOrDefault();

    if (suppression is null)
    {
        logger.LogWarning("No suppressions found for the specified email {Email}.", suppressionFilter.Email);
        logger.LogWarning("Retry without filter to get all suppressions.");

        suppressions = await suppressionsResource.Fetch();

        if (suppressions.Count == 0)
        {
            logger.LogWarning("No suppressions found.");

            return;
        }

        suppression = suppressions.First();
    }

    // Get resource for specific suppression
    ISuppressionResource suppressionResource = accountResource.Suppression(suppression.Id);

    // Delete the suppression
    Suppression? deletedSuppressionDetails = await suppressionResource.Delete();

    logger.LogInformation("Deleted suppression: {Suppression}", deletedSuppressionDetails);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.ExitCode = 1;
    return;
}
