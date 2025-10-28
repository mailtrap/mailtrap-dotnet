namespace Mailtrap.UnitTests;


[TestFixture]
internal sealed class MailtrapClientTests
{
    private readonly IRestResourceCommandFactory _commandFactoryMock = Mock.Of<IRestResourceCommandFactory>();


    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenEmailFactoryIsNull()
    {
        // Act
        var act = () => new MailtrapClient(null!, _commandFactoryMock);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenCommandFactoryIsNull()
    {
        // Act
        var act = () => new MailtrapClient(Mock.Of<IEmailClientFactory>(), null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ResourceUri_ShouldBeInitializedProperly()
    {
        // Arrange
        var client = new MailtrapClient(Mock.Of<IEmailClientFactory>(), _commandFactoryMock);

        // Assert
        client.ResourceUri.Should().Be(EndpointsTestConstants.ApiDefaultUrl.Append(UrlSegmentsTestConstants.ApiRootSegment));
    }


    [Test]
    public void Email_ShouldReturnDefaultEmailClient()
    {
        // Arrange
        var emailClientFactoryMock = new Mock<IEmailClientFactory>();
        var emailClient = Mock.Of<ISendEmailClient>();
        emailClientFactoryMock
            .Setup(f => f.CreateDefault())
            .Returns(emailClient);
        var client = new MailtrapClient(emailClientFactoryMock.Object, _commandFactoryMock);

        // Act
        var result = client.Email();

        // Assert
        result.Should().BeSameAs(emailClient);
    }

    [Test]
    public void Transactional_ShouldReturnNewEmailClient()
    {
        // Arrange
        var emailClientFactoryMock = new Mock<IEmailClientFactory>();
        var emailClient = Mock.Of<ISendEmailClient>();
        emailClientFactoryMock
            .Setup(f => f.CreateTransactional())
            .Returns(emailClient);
        var client = new MailtrapClient(emailClientFactoryMock.Object, _commandFactoryMock);

        // Act
        var result = client.Transactional();

        // Assert
        result.Should().BeSameAs(emailClient);
    }

    [Test]
    public void Bulk_ShouldReturnNewBulkEmailClient()
    {
        // Arrange
        var emailClientFactoryMock = new Mock<IEmailClientFactory>();
        var emailClient = Mock.Of<ISendEmailClient>();
        emailClientFactoryMock
            .Setup(f => f.CreateBulk())
            .Returns(emailClient);
        var client = new MailtrapClient(emailClientFactoryMock.Object, _commandFactoryMock);

        // Act
        var result = client.Bulk();

        // Assert
        result.Should().BeSameAs(emailClient);
    }

    [Test]
    public void Test_ShouldReturnNewEmailClientWithInboxId()
    {
        // Arrange
        var emailClientFactoryMock = new Mock<IEmailClientFactory>();
        var inboxId = 123;
        var emailClient = Mock.Of<ISendEmailClient>();
        emailClientFactoryMock
            .Setup(f => f.CreateTest(inboxId))
            .Returns(emailClient);
        var client = new MailtrapClient(emailClientFactoryMock.Object, _commandFactoryMock);

        // Act
        var result = client.Test(inboxId);

        // Assert
        result.Should().BeSameAs(emailClient);
    }

    [Test]
    public void Accounts_ShouldReturnAccountCollectionResource()
    {
        // Arrange
        var client = new MailtrapClient(Mock.Of<IEmailClientFactory>(), _commandFactoryMock);

        // Act
        var result = client.Accounts();

        // Assert
        result.Should()
            .NotBeNull().And
            .BeOfType<AccountCollectionResource>();

        result.ResourceUri.Should()
            .Be(client.ResourceUri.Append(UrlSegmentsTestConstants.AccountsSegment));
    }

    [Test]
    public void Account_ShouldReturnAccountResource()
    {
        // Arrange
        var client = new MailtrapClient(Mock.Of<IEmailClientFactory>(), _commandFactoryMock);
        var accountId = TestContext.CurrentContext.Random.NextLong();

        // Act
        var result = client.Account(accountId);

        // Assert
        result.Should()
            .NotBeNull().And
            .BeOfType<AccountResource>();

        result.ResourceUri.Should()
            .Be(client.ResourceUri.Append(UrlSegmentsTestConstants.AccountsSegment).Append(accountId));
    }


    [Test]
    public void BatchEmail_ShouldReturnDefaultBatchEmailClient()
    {
        // Arrange
        var emailClientFactoryMock = new Mock<IEmailClientFactory>();
        var emailClient = Mock.Of<IBatchEmailClient>();
        emailClientFactoryMock
            .Setup(f => f.CreateBatchDefault())
            .Returns(emailClient);
        var client = new MailtrapClient(emailClientFactoryMock.Object, _commandFactoryMock);

        // Act
        var result = client.BatchEmail();

        // Assert
        result.Should().BeSameAs(emailClient);
    }

    [Test]
    public void BatchTransactional_ShouldReturnNewBatchEmailClient()
    {
        // Arrange
        var emailClientFactoryMock = new Mock<IEmailClientFactory>();
        var emailClient = Mock.Of<IBatchEmailClient>();
        emailClientFactoryMock
            .Setup(f => f.CreateBatchTransactional())
            .Returns(emailClient);
        var client = new MailtrapClient(emailClientFactoryMock.Object, _commandFactoryMock);

        // Act
        var result = client.BatchTransactional();

        // Assert
        result.Should().BeSameAs(emailClient);
    }

    [Test]
    public void BatchBulk_ShouldReturnNewBatchBulkEmailClient()
    {
        // Arrange
        var emailClientFactoryMock = new Mock<IEmailClientFactory>();
        var emailClient = Mock.Of<IBatchEmailClient>();
        emailClientFactoryMock
            .Setup(f => f.CreateBatchBulk())
            .Returns(emailClient);
        var client = new MailtrapClient(emailClientFactoryMock.Object, _commandFactoryMock);

        // Act
        var result = client.BatchBulk();

        // Assert
        result.Should().BeSameAs(emailClient);
    }

    [Test]
    public void BatchTest_ShouldReturnNewBatchEmailClientWithInboxId()
    {
        // Arrange
        var emailClientFactoryMock = new Mock<IEmailClientFactory>();
        var inboxId = 123;
        var emailClient = Mock.Of<IBatchEmailClient>();
        emailClientFactoryMock
            .Setup(f => f.CreateBatchTest(inboxId))
            .Returns(emailClient);
        var client = new MailtrapClient(emailClientFactoryMock.Object, _commandFactoryMock);

        // Act
        var result = client.BatchTest(inboxId);

        // Assert
        result.Should().BeSameAs(emailClient);
    }
}
