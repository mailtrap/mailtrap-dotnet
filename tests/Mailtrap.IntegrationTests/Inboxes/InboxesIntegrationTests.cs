namespace Mailtrap.IntegrationTests.Inboxes;


[TestFixture]
internal sealed class InboxesIntegrationTests
{
    private const string Feature = "Inboxes";

    private const string CleanSegment = "clean";
    private const string MarkReadSegment = "all_read";
    private const string ResetCredentialsSegment = "reset_credentials";
    private const string ToggleEmailAddressSegment = "toggle_email_username";
    private const string ResetEmailAddressSegment = "reset_email_username";


    private readonly long _accountId;
    private readonly Uri _resourceUri;
    private readonly MailtrapClientOptions _clientConfig;
    private readonly JsonSerializerOptions _jsonSerializerOptions;


    public InboxesIntegrationTests()
    {
        var random = TestContext.CurrentContext.Random;

        _accountId = random.NextLong();
        _resourceUri = EndpointsTestConstants.ApiDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.AccountsSegment)
            .Append(_accountId)
            .Append(UrlSegmentsTestConstants.InboxesSegment);

        var token = random.GetString();
        _clientConfig = new MailtrapClientOptions(token);
        _jsonSerializerOptions = _clientConfig.ToJsonSerializerOptions();
    }


    [Test]
    public async Task GetAll_Success()
    {
        // Arrange
        var httpMethod = HttpMethod.Get;
        var requestUri = _resourceUri.AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(_clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .Account(_accountId)
            .Inboxes()
            .GetAll()
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
    }

    [Test]
    public async Task Create_Success()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;
        var projectId = random.NextLong();

        var httpMethod = HttpMethod.Post;
        var requestUri = EndpointsTestConstants.ApiDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.AccountsSegment)
            .Append(_accountId)
            .Append(UrlSegmentsTestConstants.ProjectsSegment)
            .Append(projectId)
            .Append(UrlSegmentsTestConstants.InboxesSegment)
            .AbsoluteUri;

        var inboxName = random.GetString(50);
        var request = new CreateInboxRequest(projectId, inboxName);

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .WithJsonContent(request.ToDto(), _jsonSerializerOptions)
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(_clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .Account(_accountId)
            .Inboxes()
            .Create(request)
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
    }


    [Test]
    public async Task GetDetails_Success()
    {
        // Arrange
        var httpMethod = HttpMethod.Get;
        var inboxId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(inboxId).AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(_clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .Account(_accountId)
            .Inbox(inboxId)
            .GetDetails()
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
    }

    [Test]
    public async Task Update_Success()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethodEx.Patch;

        var inboxId = random.NextLong();
        var requestUri = _resourceUri.Append(inboxId).AbsoluteUri;

        var updatedName = random.GetString(50);
        var request = new UpdateInboxRequest()
        {
            Name = updatedName
        };

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .WithJsonContent(request.ToDto(), _jsonSerializerOptions)
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(_clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .Account(_accountId)
            .Inbox(inboxId)
            .Update(request)
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
    }

    [Test]
    public async Task Delete_Success()
    {
        // Arrange
        var httpMethod = HttpMethod.Delete;
        var inboxId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(inboxId).AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(_clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .Account(_accountId)
            .Inbox(inboxId)
            .Delete()
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
    }


    [Test]
    public async Task Clean_Success()
    {
        Task<Inbox> Act(IMailtrapClient client, long inboxId) => client.Account(_accountId).Inbox(inboxId).Clean();

        await Patch_Success(CleanSegment, Act).ConfigureAwait(false);
    }

    [Test]
    public async Task MarkAsRead_Success()
    {
        Task<Inbox> Act(IMailtrapClient client, long inboxId) => client.Account(_accountId).Inbox(inboxId).MarkAsRead();

        await Patch_Success(MarkReadSegment, Act).ConfigureAwait(false);
    }

    [Test]
    public async Task ResetCredentials_Success()
    {
        Task<Inbox> Act(IMailtrapClient client, long inboxId) => client.Account(_accountId).Inbox(inboxId).ResetCredentials();

        await Patch_Success(ResetCredentialsSegment, Act).ConfigureAwait(false);
    }

    [Test]
    public async Task ToggleEmailAddress_Success()
    {
        Task<Inbox> Act(IMailtrapClient client, long inboxId) => client.Account(_accountId).Inbox(inboxId).ToggleEmailAddress();

        await Patch_Success(ToggleEmailAddressSegment, Act).ConfigureAwait(false);
    }

    [Test]
    public async Task ResetEmailAddress_Success()
    {
        Task<Inbox> Act(IMailtrapClient client, long inboxId) => client.Account(_accountId).Inbox(inboxId).ResetEmailAddress();

        await Patch_Success(ResetEmailAddressSegment, Act).ConfigureAwait(false);
    }


    private async Task Patch_Success(string urlSegment, Func<IMailtrapClient, long, Task<Inbox>> act)
    {
        // Arrange
        var httpMethod = HttpMethodEx.Patch;
        var inboxId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri
            .Append(inboxId)
            .Append(urlSegment)
            .AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent("Patch_Success");

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(_clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await act(client, inboxId).ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
    }
}
