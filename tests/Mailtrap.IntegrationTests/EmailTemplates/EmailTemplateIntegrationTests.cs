namespace Mailtrap.IntegrationTests.EmailTemplates;


[TestFixture]
internal sealed class EmailTemplateIntegrationTests
{
    private const string Feature = "EmailTemplates";

    private readonly long _accountId;
    private readonly Uri _resourceUri;
    private readonly MailtrapClientOptions _clientConfig;
    private readonly JsonSerializerOptions _jsonSerializerOptions;


    public EmailTemplateIntegrationTests()
    {
        var random = TestContext.CurrentContext.Random;

        _accountId = random.NextLong();
        _resourceUri = EndpointsTestConstants.ApiDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.AccountsSegment)
            .Append(_accountId)
            .Append(UrlSegmentsTestConstants.EmailTemplatesSegment);

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
        var expectedResponse = await responseContent.DeserializeStringContentAsync<List<EmailTemplate>>(_jsonSerializerOptions);
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
            .EmailTemplates()
            .GetAll()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should()
            .NotBeNull().And
            .HaveCount(3);

        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Test]
    public async Task Create_Success()
    {
        // Arrange
        var httpMethod = HttpMethod.Post;
        var requestUri = _resourceUri.AbsoluteUri;
        var fileName = TestContext.CurrentContext.Test.MethodName;

        var requestContent = await Feature.LoadFileToString(fileName + "_Request");
        var request = JsonSerializer.Deserialize<CreateEmailTemplateRequest>(requestContent, _jsonSerializerOptions);
        request.Should().NotBeNull();

        using var responseContent = await Feature.LoadFileToStringContent(fileName + "_Response");
        var expectedResponse = await responseContent.DeserializeStringContentAsync<EmailTemplate>(_jsonSerializerOptions);

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
            .EmailTemplates()
            .Create(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [TestCase(256, 5, 5)]
    [TestCase(5, 256, 5)]
    [TestCase(5, 5, 256)]
    [TestCase(256, 5, 256)]
    [TestCase(256, 256, 5)]
    [TestCase(5, 256, 256)]
    [TestCase(256, 256, 256)]
    public async Task Create_ShouldFailValidation_WhenRequestIsNotValid(int nameLength, int categoryLength, int subjectLength)
    {
        // Arrange
        var httpMethod = HttpMethod.Post;
        var requestUri = _resourceUri.AbsoluteUri;

        var emailTemplateName = TestContext.CurrentContext.Random.GetString(nameLength);
        var emailTemplateCategory = TestContext.CurrentContext.Random.GetString(categoryLength);
        var emailTemplateSubject = TestContext.CurrentContext.Random.GetString(subjectLength);
        var request = new CreateEmailTemplateRequest(emailTemplateName, emailTemplateCategory, emailTemplateSubject);

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
            .EmailTemplates()
            .Create(request);

        // Assert
        await act.Should().ThrowAsync<RequestValidationException>();

        mockHttp.GetMatchCount(mockedRequest).Should().Be(0);
    }

    [TestCase(10_000_001, 5)]
    [TestCase(5, 10_000_001)]
    [TestCase(10_000_001, 10_000_001)]
    public async Task Create_ShouldFailValidation_WhenRequestWithBodyIsNotValid(int bodyTextLength, int bodyHtmlLength)
    {
        // Arrange
        var httpMethod = HttpMethod.Post;
        var requestUri = _resourceUri.AbsoluteUri;

        var emailTemplateBodyText = TestContext.CurrentContext.Random.GetString(bodyTextLength);
        var emailTemplateBodyHtml = TestContext.CurrentContext.Random.GetString(bodyHtmlLength);
        var request = new CreateEmailTemplateRequest("name", "category", "subject")
        {
            BodyText = emailTemplateBodyText,
            BodyHtml = emailTemplateBodyHtml
        };

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
            .EmailTemplates()
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
        var emailTemplateId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(emailTemplateId).AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<EmailTemplate>(_jsonSerializerOptions);

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
            .EmailTemplate(emailTemplateId)
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
        var emailTemplateId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(emailTemplateId).AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();
        var expectedResponse = await responseContent.DeserializeStringContentAsync<EmailTemplate>(_jsonSerializerOptions);

        var updatedEmailTemplateName = TestContext.CurrentContext.Random.GetString(10);
        var updatedEmailTemplateCategory = TestContext.CurrentContext.Random.GetString(10);
        var updatedEmailTemplateSubject = TestContext.CurrentContext.Random.GetString(10);
        var request = new UpdateEmailTemplateRequest(updatedEmailTemplateName, updatedEmailTemplateCategory, updatedEmailTemplateSubject)
        {
            BodyHtml = TestContext.CurrentContext.Random.GetString(10),
            BodyText = TestContext.CurrentContext.Random.GetString(10)
        };

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
            .EmailTemplate(emailTemplateId)
            .Update(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull().And.BeEquivalentTo(expectedResponse);
    }

    [TestCase(256, 5, 5)]
    [TestCase(5, 256, 5)]
    [TestCase(5, 5, 256)]
    [TestCase(256, 5, 256)]
    [TestCase(256, 256, 5)]
    [TestCase(5, 256, 256)]
    [TestCase(256, 256, 256)]
    public async Task Update_ShouldFailValidation_WhenRequestIsNotValid(int nameLength, int categoryLength, int subjectLength)
    {
        // Arrange
        var httpMethod = HttpMethodEx.Patch;
        var emailTemplateId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(emailTemplateId).AbsoluteUri;

        var emailTemplateName = TestContext.CurrentContext.Random.GetString(nameLength);
        var emailTemplateCategory = TestContext.CurrentContext.Random.GetString(categoryLength);
        var emailTemplateSubject = TestContext.CurrentContext.Random.GetString(subjectLength);
        var request = new UpdateEmailTemplateRequest(emailTemplateName, emailTemplateCategory, emailTemplateSubject);

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
            .EmailTemplate(emailTemplateId)
            .Update(request);

        // Assert
        await act.Should().ThrowAsync<RequestValidationException>();

        mockHttp.GetMatchCount(mockedRequest).Should().Be(0);
    }

    [TestCase(10_000_001, 5)]
    [TestCase(5, 10_000_001)]
    [TestCase(10_000_001, 10_000_001)]
    public async Task Update_ShouldFailValidation_WhenRequestWithBodyIsNotValid(int bodyTextLength, int bodyHtmlLength)
    {
        // Arrange
        var httpMethod = HttpMethodEx.Patch;
        var emailTemplateId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(emailTemplateId).AbsoluteUri;

        var emailTemplateBodyText = TestContext.CurrentContext.Random.GetString(bodyTextLength);
        var emailTemplateBodyHtml = TestContext.CurrentContext.Random.GetString(bodyHtmlLength);
        var request = new UpdateEmailTemplateRequest("name", "category", "subject")
        {
            BodyText = emailTemplateBodyText,
            BodyHtml = emailTemplateBodyHtml
        };

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
            .EmailTemplate(emailTemplateId)
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
        var emailTemplateId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(emailTemplateId).AbsoluteUri;

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
            .EmailTemplate(emailTemplateId)
            .Delete()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }
}
