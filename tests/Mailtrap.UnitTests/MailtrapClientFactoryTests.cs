namespace Mailtrap.UnitTests;


[TestFixture]
internal sealed class MailtrapClientFactoryTests
{
    [Test]
    public void Constructor_OptionsAndDelegate_ShouldThrowArgumentNullException_WhenConfigurationIsNull()
    {
        MailtrapClientOptions? options = null;

        var act = () => new MailtrapClientFactory(options!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_OptionsAndDelegate_ShouldCallDelegate()
    {
        var options = new MailtrapClientOptions("token");
        var configure = new Mock<Action<IHttpClientBuilder>>();

        using var factory = new MailtrapClientFactory(options, configure.Object);

        configure.Verify(c => c.Invoke(It.IsAny<IHttpClientBuilder>()));
    }


    [Test]
    public void Constructor_OptionsAndClient_ShouldThrowArgumentNullException_WhenConfigurationIsNull()
    {
        MailtrapClientOptions? options = null;
        var httpClient = Mock.Of<HttpClient>();

        var act = () => new MailtrapClientFactory(options!, httpClient);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_OptionsAndClient_ShouldThrowArgumentNullException_WhenHttpClientIsNull()
    {
        HttpClient? httpClient = null;

        var act = () => new MailtrapClientFactory(MailtrapClientOptions.Default, httpClient!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public async Task Constructor_OptionsAndClient_ShouldUseClientProvided()
    {
        var config = new MailtrapClientOptions("token");

        var httpMethod = HttpMethod.Post;
        var sendUrl = EndpointsTestConstants.SendDefaultUrl.Append(UrlSegmentsTestConstants.ApiRootSegment, UrlSegmentsTestConstants.SendEmailSegment);
        var messageId = TestContext.CurrentContext.Random.GetString();
        var response = SendEmailResponse.CreateSuccess(messageId);
        using var responseContent = JsonContent.Create(response);

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, sendUrl.AbsoluteUri)
            .Respond(HttpStatusCode.OK, responseContent);

        using var factory = new MailtrapClientFactory(config, mockHttp.ToHttpClient());

        var client = factory.CreateClient();

        var request = SendEmailRequest
            .Create()
            .From("john.doe@demomailtrap.com", "John Doe")
            .To("hero.bill@galaxy.net")
            .Subject("Invitation to Earth")
            .Text("Dear Bill,\nIt will be a great pleasure to see you on our blue planet next weekend.\nBest regards, John.");

        var _ = await client.Email().Send(request);

        mockHttp.VerifyNoOutstandingExpectation();
    }


    [Test]
    public void Constructor_KeyAndDelegate_ShouldThrowArgumentNullException_WhenApiKeyIsNull()
    {
        string? apiKey = null;

        var act = () => new MailtrapClientFactory(apiKey!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_KeyAndDelegate_ShouldCallDelegate()
    {
        var configure = new Mock<Action<IHttpClientBuilder>>();

        using var client = new MailtrapClientFactory("token", configure.Object);

        configure.Verify(c => c.Invoke(It.IsAny<IHttpClientBuilder>()));
    }


    [Test]
    public void Constructor_KeyAndClient_ShouldThrowArgumentNullException_WhenApiKeyIsNull()
    {
        string? apiKey = null;
        var httpClient = Mock.Of<HttpClient>();

        var act = () => new MailtrapClientFactory(apiKey!, httpClient);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_KeyAndClient_ShouldThrowArgumentNullException_WhenHttpClientIsNull()
    {
        HttpClient? httpClient = null;

        var act = () => new MailtrapClientFactory("token", httpClient!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public async Task Constructor_KeyAndClient_ShouldUseClientProvided()
    {
        var httpMethod = HttpMethod.Post;
        var sendUrl = EndpointsTestConstants.SendDefaultUrl.Append(UrlSegmentsTestConstants.ApiRootSegment, UrlSegmentsTestConstants.SendEmailSegment);
        var messageId = TestContext.CurrentContext.Random.GetString();
        var response = SendEmailResponse.CreateSuccess(messageId);
        using var responseContent = JsonContent.Create(response);

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, sendUrl.AbsoluteUri)
            .Respond(HttpStatusCode.OK, responseContent);

        using var factory = new MailtrapClientFactory("token", mockHttp.ToHttpClient());

        var client = factory.CreateClient();

        var request = SendEmailRequest
            .Create()
            .From("john.doe@demomailtrap.com", "John Doe")
            .To("hero.bill@galaxy.net")
            .Subject("Invitation to Earth")
            .Text("Dear Bill,\nIt will be a great pleasure to see you on our blue planet next weekend.\nBest regards, John.");

        var _ = await client.Email().Send(request);

        mockHttp.VerifyNoOutstandingExpectation();
    }


    [Test]
    public void Dispose_ShouldDisposeInternals()
    {
        var factory = new MailtrapClientFactory("token");

        factory.Dispose();

        var act = factory.CreateClient;

        act.Should().Throw<ObjectDisposedException>();
    }
}
