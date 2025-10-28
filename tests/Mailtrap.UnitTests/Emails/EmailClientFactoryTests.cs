namespace Mailtrap.UnitTests.Emails;


[TestFixture]
internal sealed class EmailClientFactoryTests
{
    #region Constructor

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenOptionsIsNull()
    {
        var emailClientEndpointProviderMock = Mock.Of<IEmailClientEndpointProvider>();
        var restResourceCommandFactoryMock = Mock.Of<IRestResourceCommandFactory>();

        var act = () => new EmailClientFactory(
            null!,
            emailClientEndpointProviderMock,
            restResourceCommandFactoryMock);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenEndpointProviderIsNull()
    {
        var optionsMock = Mock.Of<IOptions<MailtrapClientOptions>>();
        var restResourceCommandFactoryMock = Mock.Of<IRestResourceCommandFactory>();

        var act = () => new EmailClientFactory(
            optionsMock,
            null!,
            restResourceCommandFactoryMock);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenRestCommandFactoryIsNull()
    {
        var optionsMock = Mock.Of<IOptions<MailtrapClientOptions>>();
        var emailClientEndpointProviderMock = Mock.Of<IEmailClientEndpointProvider>();

        var act = () => new EmailClientFactory(
            optionsMock,
            emailClientEndpointProviderMock,
            null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion


    [Test]
    public void Create_ShouldReturnEmailClient([Values] bool isBulk, [Random(2)] long inboxId)
    {
        // Arrange
        var sendUri = new Uri("https://localhost/api/send");

        var emailClientEndpointProviderMock = new Mock<IEmailClientEndpointProvider>();
        emailClientEndpointProviderMock
            .Setup(x => x.GetRequestUri(false, isBulk, inboxId))
            .Returns(sendUri);

        var options = CreateOptions(isBulk, inboxId);
        var emailClientFactory = new EmailClientFactory(
            options,
            emailClientEndpointProviderMock.Object,
            Mock.Of<IRestResourceCommandFactory>());

        // Act
        var result = emailClientFactory.Create(isBulk, inboxId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ISendEmailClient>();
        result.ResourceUri.Should().Be(sendUri);
    }

    [Test]
    public void CreateDefault_ShouldReturnEmailClient([Values] bool isBulk, [Random(2)] long inboxId)
    {
        // Arrange
        var sendUri = new Uri("https://localhost/api/send");

        var emailClientEndpointProviderMock = new Mock<IEmailClientEndpointProvider>();
        emailClientEndpointProviderMock
            .Setup(x => x.GetRequestUri(false, isBulk, inboxId))
            .Returns(sendUri);

        var options = CreateOptions(isBulk, inboxId);
        var emailClientFactory = new EmailClientFactory(
            options,
            emailClientEndpointProviderMock.Object,
            Mock.Of<IRestResourceCommandFactory>());

        // Act
        var result = emailClientFactory.CreateDefault();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ISendEmailClient>();
        result.ResourceUri.Should().Be(sendUri);
    }

    [Test]
    public void CreateTransactional_ShouldReturnEmailClient([Values] bool isBulk, [Random(2)] long inboxId)
    {
        // Arrange
        var sendUri = new Uri("https://localhost/api/send");

        var emailClientEndpointProviderMock = new Mock<IEmailClientEndpointProvider>();
        emailClientEndpointProviderMock
            .Setup(x => x.GetRequestUri(false, false, null))
            .Returns(sendUri);

        var options = CreateOptions(isBulk, inboxId);
        var emailClientFactory = new EmailClientFactory(
            options,
            emailClientEndpointProviderMock.Object,
            Mock.Of<IRestResourceCommandFactory>());

        // Act
        var result = emailClientFactory.CreateTransactional();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ISendEmailClient>();
        result.ResourceUri.Should().Be(sendUri);
    }

    [Test]
    public void CreateBulk_ShouldReturnEmailClient([Values] bool isBulk, [Random(2)] long inboxId)
    {
        // Arrange
        var sendUri = new Uri("https://localhost/api/send");

        var emailClientEndpointProviderMock = new Mock<IEmailClientEndpointProvider>();
        emailClientEndpointProviderMock
            .Setup(x => x.GetRequestUri(false, true, null))
            .Returns(sendUri);

        var options = CreateOptions(isBulk, inboxId);
        var emailClientFactory = new EmailClientFactory(
            options,
            emailClientEndpointProviderMock.Object,
            Mock.Of<IRestResourceCommandFactory>());

        // Act
        var result = emailClientFactory.CreateBulk();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ISendEmailClient>();
        result.ResourceUri.Should().Be(sendUri);
    }

    [Test]
    public void CreateTest_ShouldReturnEmailClient([Values] bool isBulk, [Random(2)] long inboxId)
    {
        // Arrange
        var sendUri = new Uri("https://localhost/api/send");

        var emailClientEndpointProviderMock = new Mock<IEmailClientEndpointProvider>();
        emailClientEndpointProviderMock
            .Setup(x => x.GetRequestUri(false, false, inboxId))
            .Returns(sendUri);

        var options = CreateOptions(isBulk, inboxId);
        var emailClientFactory = new EmailClientFactory(
            options,
            emailClientEndpointProviderMock.Object,
            Mock.Of<IRestResourceCommandFactory>());

        // Act
        var result = emailClientFactory.CreateTest(inboxId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ISendEmailClient>();
        result.ResourceUri.Should().Be(sendUri);
    }


    [Test]
    public void CreateBatch_ShouldReturnBatchEmailClient([Values] bool isBulk, [Random(2)] long inboxId)
    {
        // Arrange
        var sendUri = new Uri("https://localhost/api/batch");

        var emailClientEndpointProviderMock = new Mock<IEmailClientEndpointProvider>();
        emailClientEndpointProviderMock
            .Setup(x => x.GetRequestUri(true, isBulk, inboxId))
            .Returns(sendUri);

        var options = CreateOptions(isBulk, inboxId);
        var emailClientFactory = new EmailClientFactory(
            options,
            emailClientEndpointProviderMock.Object,
            Mock.Of<IRestResourceCommandFactory>());

        // Act
        var result = emailClientFactory.CreateBatch(isBulk, inboxId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IBatchEmailClient>();
        result.ResourceUri.Should().Be(sendUri);
    }

    [Test]
    public void CreateBatchDefault_ShouldReturnBatchEmailClient([Values] bool isBulk, [Random(2)] long inboxId)
    {
        // Arrange
        var sendUri = new Uri("https://localhost/api/batch");

        var emailClientEndpointProviderMock = new Mock<IEmailClientEndpointProvider>();
        emailClientEndpointProviderMock
            .Setup(x => x.GetRequestUri(true, isBulk, inboxId))
            .Returns(sendUri);

        var options = CreateOptions(isBulk, inboxId);
        var emailClientFactory = new EmailClientFactory(
            options,
            emailClientEndpointProviderMock.Object,
            Mock.Of<IRestResourceCommandFactory>());

        // Act
        var result = emailClientFactory.CreateBatchDefault();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IBatchEmailClient>();
        result.ResourceUri.Should().Be(sendUri);
    }

    [Test]
    public void CreateBatchTransactional_ShouldReturnBatchEmailClient([Values] bool isBulk, [Random(2)] long inboxId)
    {
        // Arrange
        var sendUri = new Uri("https://localhost/api/batch");

        var emailClientEndpointProviderMock = new Mock<IEmailClientEndpointProvider>();
        emailClientEndpointProviderMock
            .Setup(x => x.GetRequestUri(true, false, null))
            .Returns(sendUri);

        var options = CreateOptions(isBulk, inboxId);
        var emailClientFactory = new EmailClientFactory(
            options,
            emailClientEndpointProviderMock.Object,
            Mock.Of<IRestResourceCommandFactory>());

        // Act
        var result = emailClientFactory.CreateBatchTransactional();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IBatchEmailClient>();
        result.ResourceUri.Should().Be(sendUri);
    }

    [Test]
    public void CreateBatchBulk_ShouldReturnBatchEmailClient([Values] bool isBulk, [Random(2)] long inboxId)
    {
        // Arrange
        var sendUri = new Uri("https://localhost/api/batch");

        var emailClientEndpointProviderMock = new Mock<IEmailClientEndpointProvider>();
        emailClientEndpointProviderMock
            .Setup(x => x.GetRequestUri(true, true, null))
            .Returns(sendUri);

        var options = CreateOptions(isBulk, inboxId);
        var emailClientFactory = new EmailClientFactory(
            options,
            emailClientEndpointProviderMock.Object,
            Mock.Of<IRestResourceCommandFactory>());

        // Act
        var result = emailClientFactory.CreateBatchBulk();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IBatchEmailClient>();
        result.ResourceUri.Should().Be(sendUri);
    }

    [Test]
    public void CreateBatchTest_ShouldReturnBatchEmailClient([Values] bool isBulk, [Random(2)] long inboxId)
    {
        // Arrange
        var sendUri = new Uri("https://localhost/api/batch");

        var emailClientEndpointProviderMock = new Mock<IEmailClientEndpointProvider>();
        emailClientEndpointProviderMock
            .Setup(x => x.GetRequestUri(true, false, inboxId))
            .Returns(sendUri);

        var options = CreateOptions(isBulk, inboxId);
        var emailClientFactory = new EmailClientFactory(
            options,
            emailClientEndpointProviderMock.Object,
            Mock.Of<IRestResourceCommandFactory>());

        // Act
        var result = emailClientFactory.CreateBatchTest(inboxId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IBatchEmailClient>();
        result.ResourceUri.Should().Be(sendUri);
    }


    private static IOptions<MailtrapClientOptions> CreateOptions(bool isBulk, long inboxId)
    {
        var token = "token";

        var config = new MailtrapClientOptions(token)
        {
            InboxId = inboxId,
            UseBulkApi = isBulk
        };

        var options = Options.Create(config);
        return options;
    }
}
