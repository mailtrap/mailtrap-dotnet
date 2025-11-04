namespace Mailtrap.Accounts;


/// <summary>
/// Represents account resource.
/// </summary>
public interface IAccountResource : IRestResource
{
    /// <summary>
    /// Gets account access collection resource for the account, represented by this resource instance.
    /// </summary>
    ///
    /// <returns>
    /// Account access collection resource for the account, represented by this resource instance.
    /// </returns>
    public IAccountAccessCollectionResource Accesses();

    /// <summary>
    /// Gets resource for specific account access, identified by <paramref name="accessId"/>.
    /// </summary>
    ///
    /// <param name="accessId">
    /// ID of account access to get resource for.
    /// </param>
    ///
    /// <returns>
    /// Resource for the account access with specified ID.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="accessId"/> is less than or equal to zero.
    /// </exception>
    public IAccountAccessResource Access(long accessId);


    /// <summary>
    /// Gets permissions resource for the account, represented by this resource instance.
    /// </summary>
    ///
    /// <returns>
    /// Permissions resource for the account, represented by this resource instance.
    /// </returns>
    public IPermissionsResource Permissions();


    /// <summary>
    /// Gets billing resource for the account, represented by this resource instance.
    /// </summary>
    ///
    /// <returns>
    /// Billing resource for the account, represented by this resource instance.
    /// </returns>
    public IBillingResource Billing();


    /// <summary>
    /// Gets sending domain collection resource for the account, represented by this resource instance.
    /// </summary>
    ///
    /// <returns>
    /// Sending domain collection resource for the account, represented by this resource instance.
    /// </returns>
    public ISendingDomainCollectionResource SendingDomains();

    /// <summary>
    /// Gets resource for specific sending domain, identified by <paramref name="domainId"/>.
    /// </summary>
    ///
    /// <param name="domainId">
    /// ID of sending domain to get resource for.
    /// </param>
    ///
    /// <returns>
    /// Resource for the sending domain with specified ID.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="domainId"/> is less than or equal to zero.
    /// </exception>
    public ISendingDomainResource SendingDomain(long domainId);


    /// <summary>
    /// Gets project collection resource for the account, represented by this resource instance.
    /// </summary>
    ///
    /// <returns>
    /// Project collection resource for the account, represented by this resource instance.
    /// </returns>
    public IProjectCollectionResource Projects();

    /// <summary>
    /// Gets resource for specific project, identified by <paramref name="projectId"/>.
    /// </summary>
    ///
    /// <param name="projectId">
    /// ID of project to get resource for.
    /// </param>
    ///
    /// <returns>
    /// Resource for the project with specified ID.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="projectId"/> is less than or equal to zero.
    /// </exception>
    public IProjectResource Project(long projectId);


    /// <summary>
    /// Gets inbox collection resource for the account, represented by this resource instance.
    /// </summary>
    ///
    /// <returns>
    /// Inbox collection resource for the account, represented by this resource instance.
    /// </returns>
    public IInboxCollectionResource Inboxes();

    /// <summary>
    /// Gets resource for specific inbox, identified by <paramref name="inboxId"/>.
    /// </summary>
    ///
    /// <param name="inboxId">
    /// ID of inbox to get resource for.
    /// </param>
    ///
    /// <returns>
    /// Resource for the inbox with specified ID.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="inboxId"/> is less than or equal to zero.
    /// </exception>
    public IInboxResource Inbox(long inboxId);

    /// <summary>
    /// Gets contact collection resource for the account, represented by this resource instance.
    /// </summary>
    ///
    /// <returns>
    /// Contact collection resource for the account, represented by this resource instance.
    /// </returns>
    public IContactCollectionResource Contacts();

    /// <summary>
    /// Gets resource for specific contact, identified by <paramref name="idOrEmail"/>.
    /// </summary>
    ///
    /// <param name="idOrEmail">
    /// ID or email of contact to get resource for.
    /// </param>
    ///
    /// <returns>
    /// Resource for the contact with specified ID or email.
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="idOrEmail"/> is null or empty.
    /// </exception>
    public IContactResource Contact(string idOrEmail);

    /// <summary>
    /// Gets email template collection resource for the account, represented by this resource instance.
    /// </summary>
    ///
    /// <returns>
    /// Email template collection resource for the account, represented by this resource instance.
    /// </returns>
    public IEmailTemplateCollectionResource EmailTemplates();

    /// <summary>
    /// Gets resource for specific email template, identified by <paramref name="emailTemplateId"/>.
    /// </summary>
    ///
    /// <param name="emailTemplateId">
    /// ID of email template to get resource for.
    /// </param>
    ///
    /// <returns>
    /// Resource for the email template with specified ID.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="emailTemplateId"/> is less than or equal to zero.
    /// </exception>
    public IEmailTemplateResource EmailTemplate(long emailTemplateId);

    /// <summary>
    /// Gets suppression collection resource for the account, represented by this resource instance.
    /// </summary>
    ///
    /// <returns>
    /// Suppression collection resource for the account, represented by this resource instance.
    /// </returns>
    public ISuppressionCollectionResource Suppressions();

    /// <summary>
    /// Gets resource for specific suppression, identified by <paramref name="suppressionId"/>.
    /// </summary>
    ///
    /// <param name="suppressionId">
    /// Suppression ID to get resource for.
    /// </param>
    ///
    /// <returns>
    /// Resource for the suppression with specified ID.
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="suppressionId"/> is null or empty.
    /// </exception>
    public ISuppressionResource Suppression(string suppressionId);
}
