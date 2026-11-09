using AccountAccessResource = Mailtrap.AccountAccesses.AccountAccessResource;


namespace Mailtrap.Accounts;


internal sealed class AccountResource : RestResource, IAccountResource
{
    public AccountResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public IBillingResource Billing()
        => new BillingResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.BillingSegment));


    public IPermissionsResource Permissions()
        => new PermissionsResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.PermissionsSegment));

    #region Account Accesses

    public IAccountAccessCollectionResource Accesses()
        => new AccountAccessCollectionResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.AccessesSegment));

    public IAccountAccessResource Access(long accessId)
        => new AccountAccessResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.AccessesSegment).Append(accessId));

    #endregion

    #region Domains

    public ISendingDomainCollectionResource Domains()
        => new SendingDomainCollectionResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.SendingDomainsSegment));

    public ISendingDomainResource Domain(long domainId)
        => new SendingDomainResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.SendingDomainsSegment).Append(domainId));

    [Obsolete("SendingDomains() is deprecated and will be removed in a future major release; use Domains() instead.")]
    public ISendingDomainCollectionResource SendingDomains() => Domains();

    [Obsolete("SendingDomain(long) is deprecated and will be removed in a future major release; use Domain(long) instead.")]
    public ISendingDomainResource SendingDomain(long domainId) => Domain(domainId);

    #endregion

    #region Projects

    public IProjectCollectionResource Projects()
        => new ProjectCollectionResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.ProjectsSegment));

    public IProjectResource Project(long projectId)
        => new ProjectResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.ProjectsSegment).Append(projectId));

    #endregion

    #region Sandboxes

    // Passing account resource URI is expected, since we need to append both:
    // inboxes and projects segments to it for different scenarios.
    public IInboxCollectionResource Sandboxes()
        => new InboxCollectionResource(RestResourceCommandFactory, ResourceUri);

    public IInboxResource Sandbox(long sandboxId)
        => new InboxResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.InboxesSegment).Append(sandboxId));

    [Obsolete("Inboxes() is deprecated and will be removed in a future major release; use Sandboxes() instead.")]
    public IInboxCollectionResource Inboxes() => Sandboxes();

    [Obsolete("Inbox(long) is deprecated and will be removed in a future major release; use Sandbox(long) instead.")]
    public IInboxResource Inbox(long inboxId) => Sandbox(inboxId);

    #endregion

    #region Contacts

    public IContactCollectionResource Contacts()
        => new ContactCollectionResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.ContactsSegment));

    public IContactResource Contact(string idOrEmail)
    {
        Ensure.NotNullOrEmpty(idOrEmail, nameof(idOrEmail));
        var encoded = Uri.EscapeDataString(idOrEmail);

        return new ContactResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.ContactsSegment).Append(encoded));
    }

    #endregion

    #region Email Templates

    public IEmailTemplateCollectionResource EmailTemplates()
        => new EmailTemplateCollectionResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.EmailTemplatesSegment));

    public IEmailTemplateResource EmailTemplate(long emailTemplateId)
        => new EmailTemplateResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.EmailTemplatesSegment).Append(emailTemplateId));

    #endregion

    #region Templates

    public ITemplateCollectionResource Templates()
        => new TemplateCollectionResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.TemplatesSegment));

    public ITemplateResource Template(long templateId)
    {
        Ensure.GreaterThanZero(templateId, nameof(templateId));

        return new TemplateResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.TemplatesSegment).Append(templateId));
    }

    #endregion

    #region Suppressions

    public ISuppressionCollectionResource Suppressions()
        => new SuppressionCollectionResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.SuppressionsSegment));

    public ISuppressionResource Suppression(string suppressionId)
    {
        Ensure.NotNullOrEmpty(suppressionId, nameof(suppressionId));
        var encoded = Uri.EscapeDataString(suppressionId);

        return new SuppressionResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.SuppressionsSegment).Append(encoded));
    }

    #endregion

    #region Stats

    public IStatsResource Stats()
        => new StatsResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.StatsSegment));

    #endregion

    #region Email Logs

    public IEmailLogCollectionResource EmailLogs()
        => new EmailLogCollectionResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.EmailLogsSegment));

    public IEmailLogResource EmailLog(string sendingMessageId)
    {
        Ensure.NotNullOrEmpty(sendingMessageId, nameof(sendingMessageId));
        var encoded = Uri.EscapeDataString(sendingMessageId);

        return new EmailLogResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.EmailLogsSegment).Append(encoded));
    }

    #endregion

    #region API Tokens

    public IApiTokenCollectionResource ApiTokens()
        => new ApiTokenCollectionResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.ApiTokensSegment));

    public IApiTokenResource ApiToken(long apiTokenId)
    {
        Ensure.GreaterThanZero(apiTokenId, nameof(apiTokenId));

        return new ApiTokenResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.ApiTokensSegment).Append(apiTokenId));
    }

    #endregion

    #region Webhooks

    public IWebhookCollectionResource Webhooks()
        => new WebhookCollectionResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.WebhooksSegment));

    public IWebhookResource Webhook(long webhookId)
    {
        Ensure.GreaterThanZero(webhookId, nameof(webhookId));

        return new WebhookResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.WebhooksSegment).Append(webhookId));
    }

    #endregion
}
