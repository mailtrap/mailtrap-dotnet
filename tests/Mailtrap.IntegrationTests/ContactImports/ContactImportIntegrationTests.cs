namespace Mailtrap.IntegrationTests.ContactImports;


[TestFixture]
internal sealed class ContactImportIntegrationTests
{
    private const string Feature = "ContactImports";

    private readonly long _accountId;
    private readonly Uri _resourceUri;
    private readonly MailtrapClientOptions _clientConfig;
    private readonly JsonSerializerOptions _jsonSerializerOptions;


    public ContactImportIntegrationTests()
    {
        var random = TestContext.CurrentContext.Random;

        _accountId = random.NextLong();
        _resourceUri = EndpointsTestConstants.ApiDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.AccountsSegment)
            .Append(_accountId)
            .Append(UrlSegmentsTestConstants.ContactsSegment)
            .Append(UrlSegmentsTestConstants.ImportsSegment);

        var token = random.GetString();
        _clientConfig = new MailtrapClientOptions(token);
        _jsonSerializerOptions = _clientConfig.ToJsonSerializerOptions();
    }

    [Test]
    public async Task Create_Success()
    {
        // Arrange
        var httpMethod = HttpMethod.Post;
        var requestUri = _resourceUri.AbsoluteUri;

        var fileName = TestContext.CurrentContext.Test.MethodName;

        var requestContent = await Feature.LoadFileToString(fileName + "_Request");
        var request = JsonSerializer.Deserialize<CreateContactImportRequest>(requestContent, _jsonSerializerOptions);
        request.Should().NotBeNull();

        using var responseContent = await Feature.LoadFileToStringContent(fileName + "_Response");
        var expectedResponse = await responseContent.DeserializeStringContentAsync<ContactImport>(_jsonSerializerOptions);

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .WithJsonContent(request, _jsonSerializerOptions)
            .Respond(HttpStatusCode.Created, responseContent);

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddMailtrapClient(_clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();
        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var result = await client
            .Account(_accountId)
            .Contacts()
            .Imports()
            .Create(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Test]
    public async Task Create_ShouldFailValidation_WhenProvidedCollectionSizeIsInvalid([Values(0, 50001)] int length)
    {
        // Arrange
        var httpMethod = HttpMethod.Post;
        var requestUri = _resourceUri.AbsoluteUri;

        var contacts = new List<ContactImportRequest>(length);
        for (var i = 0; i < length; i++)
        {
            contacts.Add(new ContactImportRequest(TestContext.CurrentContext.Random.NextEmail()));
        }
        var request = length == 0 ? new CreateContactImportRequest() : new CreateContactImportRequest(contacts);
        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        var mockedRequest = mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .WithJsonContent(request, _jsonSerializerOptions)
            .Respond(HttpStatusCode.UnprocessableContent, responseContent);

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddMailtrapClient(_clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();
        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var act = () => client
            .Account(_accountId)
            .Contacts()
            .Imports()
            .Create(request);

        // Assert
        await act.Should().ThrowAsync<RequestValidationException>();

        mockHttp.GetMatchCount(mockedRequest).Should().Be(0);
    }

    [Test]
    public async Task Create_ShouldFailValidation_WhenProvidedCollectionContainsNull()
    {
        // Arrange
        var httpMethod = HttpMethod.Post;
        var requestUri = _resourceUri.AbsoluteUri;

        var contacts = new List<ContactImportRequest>()
        {
            new(TestContext.CurrentContext.Random.NextEmail()),
            null!
        };

        var request = new CreateContactImportRequest(contacts);

        using var mockHttp = new MockHttpMessageHandler();
        var mockedRequest = mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .WithJsonContent(request, _jsonSerializerOptions)
            .Respond(HttpStatusCode.UnprocessableContent);

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddMailtrapClient(_clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();
        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var act = () => client
            .Account(_accountId)
            .Contacts()
            .Imports()
            .Create(request);

        // Assert
        await act.Should().ThrowAsync<RequestValidationException>();

        mockHttp.GetMatchCount(mockedRequest).Should().Be(0);
    }

    [Test]
    public async Task Create_ShouldFailValidation_WhenProvidedCollectionContainsInvalidRecord([Values(1, 101)] int emailLength)
    {
        // Arrange
        var httpMethod = HttpMethod.Post;
        var requestUri = _resourceUri.AbsoluteUri;

        var contacts = new List<ContactImportRequest>()
        {
            new(TestContext.CurrentContext.Random.NextEmail()),
            new(TestContext.CurrentContext.Random.NextEmail(emailLength)),
        };

        var request = new CreateContactImportRequest(contacts);

        using var mockHttp = new MockHttpMessageHandler();
        var mockedRequest = mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .WithJsonContent(request, _jsonSerializerOptions)
            .Respond(HttpStatusCode.UnprocessableContent);

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddMailtrapClient(_clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();
        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var act = () => client
            .Account(_accountId)
            .Contacts()
            .Imports()
            .Create(request);

        // Assert
        await act.Should().ThrowAsync<RequestValidationException>();

        mockHttp.GetMatchCount(mockedRequest).Should().Be(0);
    }

    [Test]
    public async Task GetDetails_Success()
    {
        // Arrange
        var httpMethod = HttpMethod.Get;
        var importId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(importId).AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<ContactImport>(_jsonSerializerOptions);

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
            .Contacts()
            .Import(importId)
            .GetDetails()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().BeEquivalentTo(expectedResponse);
    }
}
