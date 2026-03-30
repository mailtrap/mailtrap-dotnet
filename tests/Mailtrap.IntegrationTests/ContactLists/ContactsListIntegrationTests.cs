namespace Mailtrap.IntegrationTests.ContactLists;


[TestFixture]
internal sealed class ContactListIntegrationTests
{
    private const string Feature = "ContactLists";

    private readonly long _accountId;
    private readonly Uri _resourceUri;
    private readonly MailtrapClientOptions _clientConfig;
    private readonly JsonSerializerOptions _jsonSerializerOptions;


    public ContactListIntegrationTests()
    {
        var random = TestContext.CurrentContext.Random;

        _accountId = random.NextLong();
        _resourceUri = EndpointsTestConstants.ApiDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.AccountsSegment)
            .Append(_accountId)
            .Append(UrlSegmentsTestConstants.ContactsSegment)
            .Append(UrlSegmentsTestConstants.ListsSegment);

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
            .Contacts()
            .Lists()
            .GetAll()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should()
            .NotBeNull().And
            .HaveCount(3);
    }

    [Test]
    public async Task Create_Success()
    {
        // Arrange
        var httpMethod = HttpMethod.Post;
        var requestUri = _resourceUri.AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<ContactList>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

        var request = new ContactListRequest(expectedResponse.Name);

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .WithJsonContent(request, _jsonSerializerOptions)
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
            .Lists()
            .Create(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull().And
            .Match<ContactList>(x => x.Name == expectedResponse.Name);
    }

    [Test]
    public async Task Create_ShouldFailValidation_WhenNameIsNotValid([Values(0, 256)] int length)
    {
        // Arrange
        var httpMethod = HttpMethod.Post;
        var requestUri = _resourceUri.AbsoluteUri;

        var contactListName = TestContext.CurrentContext.Random.GetString(length);
        var request = new ContactListRequest
        {
            Name = contactListName
        };

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
            .Lists()
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
        var listId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(listId).AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<ContactList>(_jsonSerializerOptions);

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
            .List(listId)
            .GetDetails()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull().And.BeEquivalentTo(expectedResponse);
    }

    [Test]
    public async Task Update_Success()
    {
        // Arrange
        var httpMethod = HttpMethodEx.Patch;
        var listId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(listId).AbsoluteUri;

        var updatedContactListName = TestContext.CurrentContext.Random.GetString(10);
        var request = new ContactListRequest(updatedContactListName);

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<ContactList>(_jsonSerializerOptions);

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .WithJsonContent(request, _jsonSerializerOptions)
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
            .List(listId)
            .Update(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull().And.BeEquivalentTo(expectedResponse);
    }

    [Test]
    public async Task Update_ShouldFailValidation_WhenNameIsNotValid([Values(0, 256)] int length)
    {
        // Arrange
        var httpMethod = HttpMethodEx.Patch;
        var listId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(listId).AbsoluteUri;

        var updatedContactsListName = TestContext.CurrentContext.Random.GetString(length);
        var request = new ContactListRequest()
        {
            Name = updatedContactsListName
        };

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
            .List(listId)
            .Update(request);

        // Assert
        await act.Should().ThrowAsync<RequestValidationException>();

        mockHttp.GetMatchCount(mockedRequest).Should().Be(0);
    }

    [Test]
    public async Task Delete_Success()
    {
        // Arrange
        var httpMethod = HttpMethod.Delete;
        var listId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(listId).AbsoluteUri;

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .Respond(HttpStatusCode.NoContent);

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddMailtrapClient(_clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();
        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        await client
            .Account(_accountId)
            .Contacts()
            .List(listId)
            .Delete()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }
}
