namespace Mailtrap.IntegrationTests.ContactFields;


[TestFixture]
internal sealed class ContactFieldIntegrationTests
{
    private const string Feature = "ContactFields";

    private readonly long _accountId;
    private readonly Uri _resourceUri;
    private readonly MailtrapClientOptions _clientConfig;
    private readonly JsonSerializerOptions _jsonSerializerOptions;


    public ContactFieldIntegrationTests()
    {
        var random = TestContext.CurrentContext.Random;

        _accountId = random.NextLong();
        _resourceUri = EndpointsTestConstants.ApiDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.AccountsSegment)
            .Append(_accountId)
            .Append(UrlSegmentsTestConstants.ContactsSegment)
            .Append(UrlSegmentsTestConstants.FieldsSegment);

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
            .Fields()
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
        var expectedResponse = await responseContent.DeserializeStringContentAsync<ContactField>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

        var request = new CreateContactFieldRequest(expectedResponse.Name!, expectedResponse.MergeTag!, expectedResponse.DataType!);

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
            .Fields()
            .Create(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull().And.BeEquivalentTo(expectedResponse);
    }

    [Test]
    public async Task Create_ShouldFailValidation_WhenNameIsNotValid([Values(81)] int length)
    {
        // Arrange
        var httpMethod = HttpMethod.Post;
        var requestUri = _resourceUri.AbsoluteUri;

        var contactFieldName = TestContext.CurrentContext.Random.GetString(length);
        var request = new CreateContactFieldRequest(contactFieldName, "validMergeTag", ContactFieldDataType.Integer);

        using var mockHttp = new MockHttpMessageHandler();
        var mockedRequest = mockHttp
            .When(httpMethod, requestUri)
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
            .Fields()
            .Create(request);

        // Assert
        await act.Should().ThrowAsync<RequestValidationException>();

        mockHttp.GetMatchCount(mockedRequest).Should().Be(0);
    }

    [Test]
    public async Task Create_ShouldFailValidation_WhenMergeTagIsNotValid([Values(81)] int length)
    {
        // Arrange
        var httpMethod = HttpMethod.Post;
        var requestUri = _resourceUri.AbsoluteUri;

        var contactMergeTag = TestContext.CurrentContext.Random.GetString(length);
        var request = new CreateContactFieldRequest("validName", contactMergeTag, ContactFieldDataType.Float);

        using var mockHttp = new MockHttpMessageHandler();
        var mockedRequest = mockHttp
            .When(httpMethod, requestUri)
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
            .Fields()
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
        var fieldId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(fieldId).AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<ContactField>(_jsonSerializerOptions);

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
            .Field(fieldId)
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
        var fieldId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(fieldId).AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<ContactField>(_jsonSerializerOptions);
        expectedResponse.Should().NotBeNull();

        var request = new UpdateContactFieldRequest(expectedResponse.Name, expectedResponse.MergeTag);

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
            .Field(fieldId)
            .Update(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull().And.BeEquivalentTo(expectedResponse);
    }

    [Test]
    public async Task Update_ShouldFailValidation_WhenNameIsNotValid([Values(81)] int length)
    {
        // Arrange
        var httpMethod = HttpMethodEx.Patch;
        var fieldId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(fieldId).AbsoluteUri;

        var request = new UpdateContactFieldRequest(TestContext.CurrentContext.Random.GetString(length), "validMergeTag");

        using var mockHttp = new MockHttpMessageHandler();
        var mockedRequest = mockHttp
            .When(httpMethod, requestUri)
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
            .Field(fieldId)
            .Update(request);

        // Assert
        await act.Should().ThrowAsync<RequestValidationException>();

        mockHttp.GetMatchCount(mockedRequest).Should().Be(0);
    }

    [Test]
    public async Task Update_ShouldFailValidation_WhenMergeTagIsNotValid([Values(81)] int length)
    {
        // Arrange
        var httpMethod = HttpMethodEx.Patch;
        var fieldId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(fieldId).AbsoluteUri;

        var request = new UpdateContactFieldRequest("validName", TestContext.CurrentContext.Random.GetString(length));

        using var mockHttp = new MockHttpMessageHandler();
        var mockedRequest = mockHttp
            .When(httpMethod, requestUri)
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
            .Field(fieldId)
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
        var fieldId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(fieldId).AbsoluteUri;

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
            .Field(fieldId)
            .Delete()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }
}
