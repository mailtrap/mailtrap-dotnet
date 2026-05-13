using Mailtrap;
using Mailtrap.Accounts;
using Mailtrap.ApiTokens;
using Mailtrap.ApiTokens.Models;
using Mailtrap.ApiTokens.Requests;
using Mailtrap.ApiTokens.Responses;
using Mailtrap.Core.Models;
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
    var inboxId = 67890;
    IAccountResource accountResource = mailtrapClient.Account(accountId);

    // Get resource for API tokens collection
    IApiTokenCollectionResource apiTokensResource = accountResource.ApiTokens();

    // List all API tokens visible to the current API token
    IList<ApiToken> apiTokens = await apiTokensResource.GetAll();
    logger.LogInformation("Found {Count} API token(s).", apiTokens.Count);

    // Create a new API token scoped to a specific inbox with Viewer access
    var createRequest = new CreateApiTokenRequest
    {
        Name = "Demo Viewer Token"
    };
    createRequest.Resources.Add(new ApiTokenAccessRequest(ResourceType.Inbox, inboxId, AccessLevel.Viewer));

    CreateApiTokenResponse createdToken = await apiTokensResource.Create(createRequest);

    // The full token value is only returned at creation time - store it securely
    logger.LogInformation(
        "Created API Token: Id={Id}, Name={Name}, Last4={Last4}",
        createdToken.Id,
        createdToken.Name,
        createdToken.Last4Digits);
    logger.LogInformation("Full token value (store securely, returned only once): {Token}", createdToken.Token);

    // Get resource for the specific API token
    IApiTokenResource apiTokenResource = accountResource.ApiToken(createdToken.Id);

    // Get details of the API token
    ApiToken tokenDetails = await apiTokenResource.GetDetails();
    logger.LogInformation("Token details: Id={Id}, Name={Name}, CreatedBy={CreatedBy}",
        tokenDetails.Id,
        tokenDetails.Name,
        tokenDetails.CreatedBy);

    // Reset the API token - expires the current token and returns a new one with the same permissions
    ApiTokenResetResponse resetResponse = await apiTokenResource.Reset();
    logger.LogInformation(
        "Reset API Token: Id={Id}, NewLast4={Last4}",
        resetResponse.Id,
        resetResponse.Last4Digits);
    logger.LogInformation("New token value (store securely, returned only once): {Token}", resetResponse.Token);

    // Delete the API token
    // The API token resource becomes invalid after deletion and should not be used anymore
    await apiTokenResource.Delete();
    logger.LogInformation("API Token Deleted.");
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.ExitCode = 1;
    return;
}
