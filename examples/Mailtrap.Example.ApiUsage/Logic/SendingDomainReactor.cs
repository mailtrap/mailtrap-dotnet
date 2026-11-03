namespace Mailtrap.Example.ApiUsage.Logic;


internal sealed class SendingDomainReactor : Reactor
{
    public SendingDomainReactor(IMailtrapClient mailtrapClient, ILogger<SendingDomainReactor> logger)
        : base(mailtrapClient, logger) { }


    public async Task Process(long accountId)
    {
        IAccountResource accountResource = _mailtrapClient.Account(accountId);

        // Get resource for domains collection
        ISendingDomainCollectionResource domainsResource = accountResource.Domains();

        // Get all domains for account
        IList<SendingDomain> domains = await domainsResource.GetAll();

        var domainName = "demomailtrap.com";
        SendingDomain? domain = domains
            .FirstOrDefault(d => string.Equals(d.DomainName, domainName, StringComparison.OrdinalIgnoreCase));

        if (domain is null)
        {
            _logger.LogWarning("No domain found. Creating.");

            // Create domain
            var createDomainRequest = new CreateSendingDomainRequest(domainName);
            domain = await domainsResource.Create(createDomainRequest);
        }
        else
        {
            _logger.LogInformation("Domain found.");
        }

        // Get resource for specific domain
        ISendingDomainResource domainResource = accountResource.Domain(domain.Id);

        // Get details
        domain = await domainResource.GetDetails();
        _logger.LogInformation("Domain: {Domain}", domain);

        // Domain instructions
        var instructionsRequest = new SendingDomainInstructionsRequest("admin@demomailtrap.com");
        await domainResource.SendInstructions(instructionsRequest);
    }
}
