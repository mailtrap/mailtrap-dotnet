namespace Mailtrap.UnitTests.Emails;


[TestFixture]
internal sealed class EmailClientTests
{
    #region Constructor

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenResourceCommandFactoryIsNull()
    {
        var sendUri = new Uri("https://localhost/api/send");

        var act = () => new SendEmailClient(null!, sendUri);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenSendUriIsNull()
    {
        var restResourceCommandFactoryMock = Mock.Of<IRestResourceCommandFactory>();

        var act = () => new SendEmailClient(restResourceCommandFactoryMock, null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldCorrectlyInitializeResourceUri()
    {
        var sendUri = new Uri("https://localhost/api/send");
        var restResourceCommandFactoryMock = Mock.Of<IRestResourceCommandFactory>();

        var client = new SendEmailClient(restResourceCommandFactoryMock, sendUri);

        client.ResourceUri.Should().Be(sendUri);
    }

    #endregion


    [Test]
    public async Task Send_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        var client = CreateEmailClient();

        var act = () => client.Send(null!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task Send_ShouldCallPostWithRequestInformation()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        var sendUri = new Uri("https://localhost/api/send");

        var request = CreateValidRequest();

        var messageId = TestContext.CurrentContext.Random.NextGuid().ToString();
        var response = SendEmailResponse.CreateSuccess(messageId);

        var restResourceCommandMock = new Mock<IRestResourceCommand<SendEmailResponse>>();
        restResourceCommandMock
            .Setup(c => c.Execute(cts.Token))
            .ReturnsAsync(response);

        var restResourceCommandFactoryMock = new Mock<IRestResourceCommandFactory>();
        restResourceCommandFactoryMock
            .Setup(f => f.CreatePost<SendEmailRequest, SendEmailResponse>(sendUri, request))
            .Returns(restResourceCommandMock.Object);

        var client = new SendEmailClient(restResourceCommandFactoryMock.Object, sendUri);


        // Act
        var result = await client.Send(request, cts.Token);


        // Assert
        result.Should()
            .NotBeNull().And
            .BeEquivalentTo(response);

        result!.Success.Should().BeTrue();
        result!.MessageIds.Should().ContainSingle(m => m == messageId);
    }


    private static SendEmailClient CreateEmailClient()
        => new(Mock.Of<IRestResourceCommandFactory>(), new Uri("https://localhost"));

    private static SendEmailRequest CreateValidRequest()
    {
        return SendEmailRequest
            .Create()
            .From("john.doe@demomailtrap.com", "John Doe")
            .To("hero.bill@galaxy.net")
            .Subject("Invitation to Earth")
            .Text("Dear Bill,\nIt will be a great pleasure to see you on our blue planet next weekend.\nBest regards, John.");
    }
}
