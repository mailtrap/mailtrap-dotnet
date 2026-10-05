namespace Mailtrap.IntegrationTests.Templates;


[TestFixture]
internal sealed class TemplateIntegrationTests
{
    private const string Feature = "Templates";

    private const string TokenQueryParameter = "token";
    private const string PerPageQueryParameter = "per_page";

    private readonly long _accountId;
    private readonly Uri _resourceUri;
    private readonly MailtrapClientOptions _clientConfig;
    private readonly JsonSerializerOptions _jsonSerializerOptions;


    public TemplateIntegrationTests()
    {
        var random = TestContext.CurrentContext.Random;

        _accountId = random.NextLong(1, long.MaxValue);
        _resourceUri = EndpointsTestConstants.ApiDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.AccountsSegment)
            .Append(_accountId)
            .Append(UrlSegmentsTestConstants.TemplatesSegment);

        _clientConfig = new MailtrapClientOptions(random.GetString());
        _jsonSerializerOptions = _clientConfig.ToJsonSerializerOptions();
    }


    [Test]
    public async Task GetAll_Success()
    {
        // Arrange
        var requestUri = _resourceUri.AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(HttpMethod.Get, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .Respond(HttpStatusCode.OK, responseContent);

        using var services = BuildServices(mockHttp);
        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var result = await client
            .Account(_accountId)
            .Templates()
            .GetAll()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Data.Should().HaveCount(2);

        var template = result.Data[0];
        template.Id.Should().Be(26730);
        template.Uuid.Should().Be("018dd5e3-f6d2-7c00-8f9b-e5c3f2d8a132");
        template.Name.Should().Be("Welcome");
        template.Subject.Should().Be("Welcome aboard");
        template.Category.Should().Be("Onboarding");
        template.BodyHtml.Should().Be("<div>Welcome</div>");
        template.BodyText.Should().Be("Welcome");
        template.CreatedAt.Should().NotBeNull();
        template.UpdatedAt.Should().NotBeNull();

        result.Pagination.Should().NotBeNull();
        result.Pagination!.Token.Should().Be(1);
        result.Pagination.NextToken.Should().Be(2);
        result.Pagination.PrevToken.Should().BeNull();
    }

    [Test]
    public async Task GetAll_WithFilter_SerializesPerPageAndToken()
    {
        // Arrange
        var requestUri = _resourceUri.AbsoluteUri;

        var page = 2;
        var perPage = 25;
        var filter = new TemplateListFilter { Token = page, PerPage = perPage };

        using var responseContent = await Feature.LoadFileToStringContent(fileName: "GetAll_Success");

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(HttpMethod.Get, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .WithQueryString(PerPageQueryParameter, perPage.ToString(CultureInfo.InvariantCulture))
            .WithQueryString(TokenQueryParameter, page.ToString(CultureInfo.InvariantCulture))
            .Respond(HttpStatusCode.OK, responseContent);

        using var services = BuildServices(mockHttp);
        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var result = await client
            .Account(_accountId)
            .Templates()
            .GetAll(filter)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Data.Should().HaveCount(2);
    }

    [Test]
    public async Task Create_Success()
    {
        // Arrange
        var requestUri = _resourceUri.AbsoluteUri;

        var request = new CreateTemplateRequest
        {
            Name = "My Template",
            Category = "Promotional",
            Subject = "Promotional Email",
            BodyHtml = "<div>Template Body</div>",
            BodyText = "Template Body"
        };

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(HttpMethod.Post, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            // Body must be flat - no "email_template" envelope.
            .WithJsonContent(request, _jsonSerializerOptions)
            .Respond(HttpStatusCode.Created, responseContent);

        using var services = BuildServices(mockHttp);
        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var result = await client
            .Account(_accountId)
            .Templates()
            .Create(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Id.Should().Be(26732);
        result.Name.Should().Be("My Template");
        result.Category.Should().Be("Promotional");
        result.Subject.Should().Be("Promotional Email");
        result.BodyHtml.Should().Be("<div>Template Body</div>");
    }

    [Test]
    public async Task Create_ShouldFailValidation_WhenRequestIsNotValid()
    {
        // Arrange
        var requestUri = _resourceUri.AbsoluteUri;

        var request = new CreateTemplateRequest
        {
            Name = TestContext.CurrentContext.Random.GetString(256),
            Category = "Promotional",
            Subject = "Promotional Email"
        };

        using var mockHttp = new MockHttpMessageHandler();
        var mockedRequest = mockHttp
            .Expect(HttpMethod.Post, requestUri)
            .Respond(HttpStatusCode.UnprocessableContent);

        using var services = BuildServices(mockHttp);
        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var act = () => client
            .Account(_accountId)
            .Templates()
            .Create(request);

        // Assert
        await act.Should().ThrowAsync<RequestValidationException>();

        mockHttp.GetMatchCount(mockedRequest).Should().Be(0);
    }

    [Test]
    public async Task GetDetails_Success()
    {
        // Arrange
        var templateId = 26730;
        var requestUri = _resourceUri.Append(templateId).AbsoluteUri;

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(HttpMethod.Get, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .Respond(HttpStatusCode.OK, responseContent);

        using var services = BuildServices(mockHttp);
        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var result = await client
            .Account(_accountId)
            .Template(templateId)
            .GetDetails()
            .ConfigureAwait(false);

        // Assert - the single template must be unwrapped from the "data" envelope.
        mockHttp.VerifyNoOutstandingExpectation();

        result.Id.Should().Be(templateId);
        result.Name.Should().Be("Welcome");
        result.Subject.Should().Be("Welcome aboard");
    }

    [Test]
    public async Task Update_Success()
    {
        // Arrange
        var templateId = 26730;
        var requestUri = _resourceUri.Append(templateId).AbsoluteUri;

        var request = new UpdateTemplateRequest { Name = "Welcome v2" };

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(HttpMethodEx.Patch, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            // Body must be flat and carry only the changed fields.
            .WithContent("{\"name\":\"Welcome v2\"}")
            .Respond(HttpStatusCode.OK, responseContent);

        using var services = BuildServices(mockHttp);
        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var result = await client
            .Account(_accountId)
            .Template(templateId)
            .Update(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Id.Should().Be(templateId);
        result.Name.Should().Be("Welcome v2");
    }

    [Test]
    public async Task Update_ShouldFailValidation_WhenRequestIsNotValid()
    {
        // Arrange
        var templateId = 26730;
        var requestUri = _resourceUri.Append(templateId).AbsoluteUri;

        var request = new UpdateTemplateRequest { Subject = string.Empty };

        using var mockHttp = new MockHttpMessageHandler();
        var mockedRequest = mockHttp
            .Expect(HttpMethodEx.Patch, requestUri)
            .Respond(HttpStatusCode.UnprocessableContent);

        using var services = BuildServices(mockHttp);
        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var act = () => client
            .Account(_accountId)
            .Template(templateId)
            .Update(request);

        // Assert
        await act.Should().ThrowAsync<RequestValidationException>();

        mockHttp.GetMatchCount(mockedRequest).Should().Be(0);
    }

    [Test]
    public async Task Delete_Success()
    {
        // Arrange
        var templateId = 26730;
        var requestUri = _resourceUri.Append(templateId).AbsoluteUri;

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(HttpMethod.Delete, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .Respond(HttpStatusCode.NoContent);

        using var services = BuildServices(mockHttp);
        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        await client
            .Account(_accountId)
            .Template(templateId)
            .Delete()
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }


    private ServiceProvider BuildServices(MockHttpMessageHandler mockHttp)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(_clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        return serviceCollection.BuildServiceProvider();
    }
}
