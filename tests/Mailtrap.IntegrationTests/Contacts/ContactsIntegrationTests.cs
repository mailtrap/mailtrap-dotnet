namespace Mailtrap.IntegrationTests.Contacts;


[TestFixture]
internal sealed class ContactsIntegrationTests
{
    private const string Feature = "Contacts";

    private readonly long _accountId;
    private readonly Uri _resourceUri;
    private readonly MailtrapClientOptions _clientConfig;
    private readonly JsonSerializerOptions _jsonSerializerOptions;


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

        // The API leaves the key out when topic preferences are not available for the account.
        result.Contact.SubscriptionTopics.Should().BeNull();
    }

    [Test]
    public async Task GetDetails_WithSubscriptionTopics_Success()
    {
        // Arrange
        var contactId = TestContext.CurrentContext.Random.NextGuid().ToString();

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Get,
            _resourceUri.Append(contactId).AbsoluteUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig);

        // Act
        var result = await clientScope.Client
            .Account(_accountId)
            .Contact(contactId)
            .GetDetails()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Contact.SubscriptionTopics.Should().BeEquivalentTo(
        [
            Topic(ContactStatus.Unsubscribed),
            new ContactSubscriptionTopic { SubscriptionTopicId = 15, DomainId = 4321, Status = ContactStatus.Subscribed }
        ]);
    }

    [Test]
    public async Task GetDetails_WithNullSubscriptionTopics_Success()
    {
        // Arrange
        var contactId = TestContext.CurrentContext.Random.NextGuid().ToString();

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Get,
            _resourceUri.Append(contactId).AbsoluteUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig);

        // Act
        var result = await clientScope.Client
            .Account(_accountId)
            .Contact(contactId)
            .GetDetails()
            .ConfigureAwait(false);

        // Assert - null means the preferences couldn't be read this time; it must not become an empty list.
        mockHttp.VerifyNoOutstandingExpectation();

        result.Contact.Email.Should().Be("john.smith@example.com");
        result.Contact.SubscriptionTopics.Should().BeNull();
    }

    [Test]
    public async Task Create_WithSubscriptionTopics_Success()
    {
        // Arrange
        var request = new CreateContactRequest("john.smith@example.com")
        {
            SubscriptionTopics = [Topic(ContactStatus.Unsubscribed)]
        };

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Post,
            _resourceUri.AbsoluteUri,
            responseContent,
            HttpStatusCode.Created,
            _clientConfig,
            request.ToDto());

        // Act
        var result = await clientScope.Client
            .Account(_accountId)
            .Contacts()
            .Create(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Contact.SubscriptionTopics.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(request.SubscriptionTopics[0]);
    }

    [Test]
    public async Task Create_WithSubscriptionTopics_ServiceUnavailable()
    {
        // Arrange
        var request = new CreateContactRequest("john.smith@example.com")
        {
            SubscriptionTopics = [Topic(ContactStatus.Unsubscribed)]
        };

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethod.Post,
            _resourceUri.AbsoluteUri,
            responseContent,
            HttpStatusCode.ServiceUnavailable,
            _clientConfig);

        var act = () => clientScope.Client
            .Account(_accountId)
            .Contacts()
            .Create(request);

        // Assert - the message names the contact to send the topics to again.
        await act.Should()
            .ThrowAsync<HttpRequestFailedException>()
            .Where(ex => ex.StatusCode == HttpStatusCode.ServiceUnavailable)
            .WithMessage("*Error details: 'The contact was saved*Send them again in an update of contact 018dd5e3-f6d2-7c00-8f9b-e5c3f2d8a132*");

        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Test]
    public async Task Update_WithSubscriptionTopics_Success()
    {
        // Arrange - a topic-only update: nothing but the email and the topic entries.
        var contactId = TestContext.CurrentContext.Random.NextGuid().ToString();
        var request = new UpdateContactRequest("john.smith@example.com")
        {
            SubscriptionTopics = [Topic(ContactStatus.Subscribed)]
        };

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethodEx.Patch,
            _resourceUri.Append(contactId).AbsoluteUri,
            responseContent,
            HttpStatusCode.OK,
            _clientConfig,
            request.ToDto());

        // Act
        var result = await clientScope.Client
            .Account(_accountId)
            .Contact(contactId)
            .Update(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Action.Should().Be(ContactAction.Updated);
        result.Contact.SubscriptionTopics.Should().ContainSingle()
            .Which.Status.Should().Be(ContactStatus.Subscribed);
    }

    [Test]
    public async Task Update_WithSubscriptionTopics_Unprocessable()
    {
        // Arrange
        var contactId = TestContext.CurrentContext.Random.NextGuid().ToString();
        var request = new UpdateContactRequest("john.smith@example.com")
        {
            SubscriptionTopics = [Topic(ContactStatus.Subscribed)]
        };

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        using var clientScope = mockHttp.ConfigureAndCreateClient(
            HttpMethodEx.Patch,
            _resourceUri.Append(contactId).AbsoluteUri,
            responseContent,
            HttpStatusCode.UnprocessableContent,
            _clientConfig);

        var act = () => clientScope.Client
            .Account(_accountId)
            .Contact(contactId)
            .Update(request);

        // Assert
        await act.Should()
            .ThrowAsync<HttpRequestFailedException>()
            .Where(ex => ex.StatusCode == HttpStatusCode.UnprocessableContent)
            .WithMessage("*are not available for this account*");

        mockHttp.VerifyNoOutstandingExpectation();
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


    private static ContactSubscriptionTopic Topic(ContactStatus status)
        => new() { SubscriptionTopicId = 12, DomainId = 4321, Status = status };
}
