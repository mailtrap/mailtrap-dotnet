namespace Mailtrap.IntegrationTests.Contacts;


[TestFixture]
internal sealed class ContactsIntegrationTests
{
    private const string Feature = "Contacts";

    private readonly long _accountId;
    private readonly Uri _resourceUri = null!;
    private readonly MailtrapClientOptions _clientConfig = null!;
    private readonly JsonSerializerOptions _jsonSerializerOptions = null!;


    public ContactsIntegrationTests()
    {
        var random = TestContext.CurrentContext.Random;

        _accountId = random.NextLong();
        _resourceUri = EndpointsTestConstants.ApiDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.AccountsSegment)
            .Append(_accountId)
            .Append(UrlSegmentsTestConstants.ContactsSegment);

        var token = random.GetString();
        _clientConfig = new MailtrapClientOptions(token);
        _jsonSerializerOptions = _clientConfig.ToJsonSerializerOptions();
    }

    // TODO: Enable when GetAll is implemented
    // [Test]
    // public async Task GetAll_Success()
    // {
    //     // Arrange
    //     var httpMethod = HttpMethod.Get;
    //     var requestUri = _resourceUri.AbsoluteUri;

    //     using var responseContent = await Feature.LoadFileToStringContent();

    //     using var mockHttp = new MockHttpMessageHandler();
    //     mockHttp
    //         .Expect(httpMethod, requestUri)
    //         .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
    //         .WithHeaders("Accept", MimeTypes.Application.Json)
    //         .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
    //         .Respond(HttpStatusCode.OK, responseContent);

    //     var serviceCollection = new ServiceCollection();
    //     serviceCollection
    //         .AddMailtrapClient(_clientConfig)
    //         .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

    //     using var services = serviceCollection.BuildServiceProvider();
    //     var client = services.GetRequiredService<IMailtrapClient>();

    //     // Act
    //     var result = await client
    //         .Account(_accountId)
    //         .Contacts()
    //         .GetAll()
    //         .ConfigureAwait(false);

    //     // Assert
    //     mockHttp.VerifyNoOutstandingExpectation();

    //     result.Should()
    //         .NotBeNull().And
    //         .HaveCount(3);
    // }

    [Test]
    public async Task Create_Success()
    {
        // Arrange
        var httpMethod = HttpMethod.Post;
        var requestUri = _resourceUri.AbsoluteUri;

        var contactEmail = TestContext.CurrentContext.Random.NextEmail();
        var request = new CreateContactRequest(contactEmail);

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<CreateContactResponse>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .WithJsonContent(request.ToDto(), _jsonSerializerOptions)
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
            .Create(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.ShouldBeEquivalentToResponse(expectedResponse);
    }

    [Test]
    public async Task Create_ShouldFailValidation_WhenEmailIsNotValid([Values(1, 101)] int length)
    {
        // Arrange
        var httpMethod = HttpMethod.Post;
        var requestUri = _resourceUri.AbsoluteUri;

        var contactEmail = TestContext.CurrentContext.Random.GetString(length);
        var request = new CreateContactRequest(contactEmail);

        using var mockHttp = new MockHttpMessageHandler();
        var mockedRequest = mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .WithJsonContent(request.ToDto(), _jsonSerializerOptions)
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
        var contactId = TestContext.CurrentContext.Random.NextGuid().ToString();
        var requestUri = _resourceUri.Append(contactId).AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<ContactResponse>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

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
            .Contact(contactId)
            .GetDetails()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.ShouldBeEquivalentToResponse(expectedResponse);
    }

    [Test]
    public async Task Update_Success()
    {
        // Arrange
        var httpMethod = HttpMethodEx.Patch;
        var contactId = TestContext.CurrentContext.Random.NextGuid().ToString();
        var requestUri = _resourceUri.Append(contactId).AbsoluteUri;

        var updatedEmail = TestContext.CurrentContext.Random.NextEmail(10);
        var request = new UpdateContactRequest(updatedEmail);

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<UpdateContactResponse>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

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
            .Contact(contactId)
            .Update(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.ShouldBeEquivalentToResponse(expectedResponse);
    }

    [Test]
    public async Task Update_ShouldFailValidation_WhenEmailIsNotValid([Values(1, 101)] int length)
    {
        // Arrange
        var httpMethod = HttpMethodEx.Patch;
        var contactId = TestContext.CurrentContext.Random.NextGuid().ToString();
        var requestUri = _resourceUri.Append(contactId).AbsoluteUri;

        var updatedEmail = TestContext.CurrentContext.Random.GetString(length);
        var request = new UpdateContactRequest(updatedEmail);

        using var mockHttp = new MockHttpMessageHandler();
        var mockedRequest = mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .WithJsonContent(request.ToDto(), _jsonSerializerOptions)
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
            .Contact(contactId)
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
        var contactId = TestContext.CurrentContext.Random.NextGuid().ToString();
        var requestUri = _resourceUri.Append(contactId).AbsoluteUri;

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
            .Contact(contactId)
            .Delete()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }
}
