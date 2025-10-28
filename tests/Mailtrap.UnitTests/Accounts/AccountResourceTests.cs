using AccountAccessResource = Mailtrap.AccountAccesses.AccountAccessResource;


namespace Mailtrap.UnitTests.Accounts;


[TestFixture]
internal sealed class AccountResourceTests
{
    private readonly IRestResourceCommandFactory _commandFactoryMock = Mock.Of<IRestResourceCommandFactory>();
    private readonly Uri _resourceUri = EndpointsTestConstants.ApiDefaultUrl
        .Append(
            UrlSegmentsTestConstants.ApiRootSegment,
            UrlSegmentsTestConstants.AccountsSegment)
        .Append(TestContext.CurrentContext.Random.NextLong());


    #region Constructor

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenCommandFactoryIsNull()
    {
        // Act
        var act = () => new AccountResource(null!, _resourceUri);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenUriIsNull()
    {
        // Act
        var act = () => new AccountResource(_commandFactoryMock, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ResourceUri_ShouldBeInitializedProperly()
    {
        // Arrange
        var client = CreateResource();

        // Assert
        client.ResourceUri.Should().Be(_resourceUri);
    }

    #endregion


    #region Billing

    [Test]
    public void Billing_ShouldReturnBillingResource()
    {
        // Arrange
        var client = CreateResource();

        // Act
        var result = client.Billing();

        // Assert
        ResourceValidator.Validate<IBillingResource, BillingResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.BillingSegment));
    }

    #endregion


    #region Permissions

    [Test]
    public void Permissions_ShouldReturnPermissionsResource()
    {
        // Arrange
        var client = CreateResource();

        // Act
        var result = client.Permissions();

        // Assert
        ResourceValidator.Validate<IPermissionsResource, PermissionsResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.PermissionsSegment));
    }

    #endregion


    #region Account Accesses

    [Test]
    public void Accesses_ShouldReturnAccessCollectionResource()
    {
        // Arrange
        var client = CreateResource();

        // Act
        var result = client.Accesses();

        // Assert
        ResourceValidator.Validate<IAccountAccessCollectionResource, AccountAccessCollectionResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.AccountAccessesSegment));
    }

    [Test]
    public void Access_ShouldReturnAccessResource()
    {
        // Arrange
        var client = CreateResource();
        var accessId = TestContext.CurrentContext.Random.NextLong();

        // Act
        var result = client.Access(accessId);

        // Assert
        ResourceValidator.Validate<IAccountAccessResource, AccountAccessResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.AccountAccessesSegment).Append(accessId));
    }

    [Test]
    public void Access_ShouldThrowOutOfRangeException_WhenIdIsEqualOrLessThanZero([Values(0, -1)] long accessId)
    {
        // Arrange
        var client = CreateResource();

        // Act
        var act = () => client.Access(accessId);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion


    #region Sending Domains

    [Test]
    public void SendingDomains_ShouldReturnSendingDomainCollectionResource()
    {
        // Arrange
        var client = CreateResource();

        // Act
        var result = client.SendingDomains();

        // Assert
        ResourceValidator.Validate<ISendingDomainCollectionResource, SendingDomainCollectionResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.SendingDomainsSegment));
    }

    [Test]
    public void SendingDomain_ShouldReturnSendingDomainResource()
    {
        // Arrange
        var client = CreateResource();
        var domainId = TestContext.CurrentContext.Random.NextLong();

        // Act
        var result = client.SendingDomain(domainId);

        // Assert
        ResourceValidator.Validate<ISendingDomainResource, SendingDomainResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.SendingDomainsSegment).Append(domainId));
    }

    [Test]
    public void SendingDomain_ShouldThrowOutOfRangeException_WhenIdIsEqualOrLessThanZero([Values(0, -1)] long domainId)
    {
        // Arrange
        var client = CreateResource();

        // Act
        var act = () => client.SendingDomain(domainId);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion


    #region Projects

    [Test]
    public void Projects_ShouldReturnProjectCollectionResource()
    {
        // Arrange
        var client = CreateResource();

        // Act
        var result = client.Projects();

        // Assert
        ResourceValidator.Validate<IProjectCollectionResource, ProjectCollectionResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.ProjectsSegment));
    }

    [Test]
    public void Project_ShouldReturnProjectResource()
    {
        // Arrange
        var client = CreateResource();
        var projectId = TestContext.CurrentContext.Random.NextLong();

        // Act
        var result = client.Project(projectId);

        // Assert
        ResourceValidator.Validate<IProjectResource, ProjectResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.ProjectsSegment).Append(projectId));
    }

    [Test]
    public void Project_ShouldThrowOutOfRangeException_WhenIdIsEqualOrLessThanZero([Values(0, -1)] long projectId)
    {
        // Arrange
        var client = CreateResource();

        // Act
        var act = () => client.Project(projectId);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion


    #region Inboxes

    [Test]
    public void Inboxes_ShouldReturnInboxCollectionResource()
    {
        // Arrange
        var client = CreateResource();

        // Act
        var result = client.Inboxes();

        // Assert
        ResourceValidator.Validate<IInboxCollectionResource, InboxCollectionResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.InboxesSegment));
    }

    [Test]
    public void Inbox_ShouldReturnInboxResource()
    {
        // Arrange
        var client = CreateResource();
        var inboxId = TestContext.CurrentContext.Random.NextLong();

        // Act
        var result = client.Inbox(inboxId);

        // Assert
        ResourceValidator.Validate<IInboxResource, InboxResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.InboxesSegment).Append(inboxId));
    }

    [Test]
    public void Inbox_ShouldThrowOutOfRangeException_WhenIdIsEqualOrLessThanZero([Values(0, -1)] long inboxId)
    {
        // Arrange
        var client = CreateResource();

        // Act
        var act = () => client.Inbox(inboxId);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion


    #region Contacts

    [Test]
    public void Contacts_ShouldReturnContactCollectionResource()
    {
        // Arrange
        var client = CreateResource();

        // Act
        var result = client.Contacts();

        // Assert
        ResourceValidator.Validate<IContactCollectionResource, ContactCollectionResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.ContactsSegment));
    }

    [Test]
    public void Contact_ShouldReturnContactResource()
    {
        // Arrange
        var client = CreateResource();
        var contactId = TestContext.CurrentContext.Random.NextGuid().ToString();

        // Act
        var result = client.Contact(contactId);

        // Assert
        ResourceValidator.Validate<IContactResource, ContactResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.ContactsSegment).Append(contactId));
    }

    [Test]
    public void Contact_ShouldThrowArgumentNullException_WhenIdIsNullOrEmpty([Values(null!, "")] string contactId)
    {
        // Arrange
        var client = CreateResource();

        // Act
        var act = () => client.Contact(contactId);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion


    #region Email Templates

    [Test]
    public void EmailTemplates_ShouldReturnEmailTemplateCollectionResource()
    {
        // Arrange
        var client = CreateResource();

        // Act
        var result = client.EmailTemplates();

        // Assert
        ResourceValidator.Validate<IEmailTemplateCollectionResource, EmailTemplateCollectionResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.EmailTemplatesSegment));
    }

    [Test]
    public void EmailTemplate_ShouldReturnEmailTemplateResource()
    {
        // Arrange
        var client = CreateResource();
        var emailTemplateId = TestContext.CurrentContext.Random.NextLong();

        // Act
        var result = client.EmailTemplate(emailTemplateId);

        // Assert
        ResourceValidator.Validate<IEmailTemplateResource, EmailTemplateResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.EmailTemplatesSegment).Append(emailTemplateId));
    }

    [Test]
    public void EmailTemplate_ShouldThrowOutOfRangeException_WhenIdIsEqualOrLessThanZero([Values(0, -1)] long emailTemplateId)
    {
        // Arrange
        var client = CreateResource();

        // Act
        var act = () => client.EmailTemplate(emailTemplateId);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #region Suppressions

    [Test]
    public void Suppressions_ShouldReturnSuppressionCollectionResource()
    {
        // Arrange
        var client = CreateResource();

        // Act
        var result = client.Suppressions();

        // Assert
        ResourceValidator.Validate<ISuppressionCollectionResource, SuppressionCollectionResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.SuppressionsSegment));
    }

    [Test]
    public void Suppression_ShouldReturnSuppressionResource()
    {
        // Arrange
        var client = CreateResource();
        var suppressionId = TestContext.CurrentContext.Random.NextGuid().ToString();

        // Act
        var result = client.Suppression(suppressionId);

        // Assert
        ResourceValidator.Validate<ISuppressionResource, SuppressionResource>(
            result, client.ResourceUri.Append(UrlSegmentsTestConstants.SuppressionsSegment).Append(suppressionId));
    }

    [Test]
    public void Suppression_ShouldThrowArgumentNullException_WhenIdWhenIdIsNullOrEmpty([Values(null!, "")] string suppressionId)
    {
        // Arrange
        var client = CreateResource();

        // Act
        var act = () => client.Suppression(suppressionId);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion


    private AccountResource CreateResource() => new(_commandFactoryMock, _resourceUri);
}
