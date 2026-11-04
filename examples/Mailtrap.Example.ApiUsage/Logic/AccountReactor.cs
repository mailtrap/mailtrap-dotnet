namespace Mailtrap.Example.ApiUsage.Logic;


internal sealed class AccountReactor : Reactor
{
    private readonly BillingReactor _billingReactor;
    private readonly PermissionsReactor _permissionsReactor;
    private readonly AccountAccessReactor _accountAccessReactor;
    private readonly DomainReactor _domainReactor;
    private readonly ProjectReactor _projectReactor;


    public AccountReactor(
        BillingReactor billingReactor,
        PermissionsReactor permissionsReactor,
        AccountAccessReactor accountAccessReactor,
        DomainReactor domainReactor,
        ProjectReactor projectReactor,
        IMailtrapClient mailtrapClient,
        ILogger<AccountReactor> logger)
        : base(mailtrapClient, logger)
    {
        _billingReactor = billingReactor;
        _permissionsReactor = permissionsReactor;
        _accountAccessReactor = accountAccessReactor;
        _domainReactor = domainReactor;
        _projectReactor = projectReactor;
    }


    public async Task Process(long accountId)
    {
        // Get all accounts available for the token
        IList<Account> accounts = await _mailtrapClient
            .Accounts()
            .GetAll();

        Account? account = accounts.FirstOrDefault(a => a.Id == accountId);

        if (account is null)
        {
            _logger.LogWarning("No account found.");

            return;
        }

        _logger.LogInformation("Account: {Account}", account);

        await _billingReactor.Process(accountId);
        await _permissionsReactor.Process(accountId);
        await _accountAccessReactor.Process(accountId);
        await _domainReactor.Process(accountId);
        await _projectReactor.Process(accountId);
    }
}
