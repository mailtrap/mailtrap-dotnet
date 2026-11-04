using Mailtrap;
using Mailtrap.Accounts;
using Mailtrap.SendingDomains;
using Mailtrap.SendingDomains.Models;
using Mailtrap.SendingDomains.Requests;
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

    // Get resource for domains collection
    ISendingDomainCollectionResource domainsResource = accountResource.Domains();

    // Get all domains for account
    IList<SendingDomain> domains = await domainsResource.GetAll();

    var domainName = "demomailtrap.com";
    SendingDomain? domain = domains
        .FirstOrDefault(d => string.Equals(d.DomainName, domainName, StringComparison.OrdinalIgnoreCase));

    if (domain is null)
    {
        logger.LogWarning("No domain found. Creating.");

        // Create domain
        var createDomainRequest = new CreateSendingDomainRequest(domainName);
        domain = await domainsResource.Create(createDomainRequest);
    }
    else
    {
        logger.LogInformation("Domain found.");
    }

    // Get resource for specific domain
    ISendingDomainResource domainResource = accountResource.Domain(domain.Id);

    // Get details
    domain = await domainResource.GetDetails();
    logger.LogInformation("Domain: {Domain}", domain);

    // Update domain configuration
    var updateDomainRequest = new UpdateSendingDomainRequest
    {
        OpenTrackingEnabled = true,
        ClickTrackingEnabled = true,
        TrackingOptOutEnabled = true,
        AutoUnsubscribeLinkEnabled = false
    };
    domain = await domainResource.Update(updateDomainRequest);
    logger.LogInformation("Updated Domain: {Domain}", domain);

    // Company info is token-scoped, so it hangs off the client rather than the account
    ICompanyInfoResource companyInfoResource = mailtrapClient.CompanyInfo(domain.Id);

    var createCompanyInfoRequest = new CreateCompanyInfoRequest
    {
        Name = "Mailtrap",
        Address = "123 Main St",
        City = "San Francisco",
        Country = "US",
        ZipCode = "94105",
        WebsiteUrl = new Uri("https://mailtrap.io"),
        InfoLevel = CompanyInfoLevel.Business
    };
    var companyInfo = await companyInfoResource.Create(createCompanyInfoRequest);
    logger.LogInformation("Company Info: {CompanyInfo}", companyInfo);

    var updateCompanyInfoRequest = new UpdateCompanyInfoRequest
    {
        City = "New York",
        ZipCode = "10001"
    };
    companyInfo = await companyInfoResource.Update(updateCompanyInfoRequest);
    logger.LogInformation("Updated Company Info: {CompanyInfo}", companyInfo);

    // Domain instructions
    var instructionsRequest = new SendingDomainInstructionsRequest("admin@demomailtrap.com");
    await domainResource.SendInstructions(instructionsRequest);

    // Delete domain
    // Beware that domain resource becomes invalid after deletion and should not be used anymore
    await domainResource.Delete();
    logger.LogInformation("Domain deleted.");
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during API call.");
    Environment.FailFast(ex.Message);
    throw;
}
