namespace Mailtrap.IntegrationTests.EmailCampaigns;


[TestFixture]
internal sealed class EmailCampaignIntegrationTests
{
    private const string Feature = "EmailCampaigns";

    private const string TokenQueryParameter = "token";
    private const string PerPageQueryParameter = "per_page";
    private const string SearchQueryParameter = "search";


    // Email campaigns are token-scoped: the resource is exposed under the account resource for
    // ergonomic discoverability, but its API path is the bare "/api/email_campaigns" - it does
    // NOT carry the "/api/accounts/{account_id}" prefix that every sibling resource uses.
    private static Uri CollectionUri => EndpointsTestConstants.ApiDefaultUrl
        .Append(
            UrlSegmentsTestConstants.ApiRootSegment,
            UrlSegmentsTestConstants.EmailCampaignsSegment);

    private static Uri CampaignUri(long campaignId) => CollectionUri.Append(campaignId);


    [Test]
    public async Task GetAll_Success()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Get;
        var accountId = random.NextLong();
        var requestUri = CollectionUri.AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        // Sanity: the campaigns URL must NOT be account-scoped.
        requestUri.Should().NotContain("/api/accounts/");
        requestUri.Should().EndWith("/api/email_campaigns");

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .Account(accountId)
            .EmailCampaigns()
            .GetAll()
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.Data.Should().ContainSingle()
            .Which.Should().Match<EmailCampaign>(c =>
                c.Id == 4567 &&
                c.Name == "Spring Sale" &&
                c.CurrentState == CampaignState.Draft &&
                c.DeliveryMode == DeliveryMode.Immediate);

        result.Pagination.Should().NotBeNull();
        result.Pagination!.Token.Should().Be(1);
        result.Pagination.NextToken.Should().Be(2);
        result.Pagination.PrevToken.Should().BeNull();
    }

    [Test]
    public async Task GetAll_WithFilter_SerializesSearchPerPageAndToken()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Get;
        var accountId = random.NextLong();
        var requestUri = CollectionUri.AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        var page = 2;
        var perPage = 25;
        var search = "Spring";

        var filter = new EmailCampaignListFilter
        {
            Token = page,
            PerPage = perPage,
            Search = search
        };

        using var responseContent = await Feature.LoadFileToStringContent(fileName: "GetAll_Success");

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            // The name filter must serialize to "search", NOT "name".
            .WithQueryString(SearchQueryParameter, search)
            .WithQueryString(PerPageQueryParameter, perPage.ToString(CultureInfo.InvariantCulture))
            .WithQueryString(TokenQueryParameter, page.ToString(CultureInfo.InvariantCulture))
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .Account(accountId)
            .EmailCampaigns()
            .GetAll(filter)
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.Data.Should().ContainSingle();
    }

    [Test]
    public async Task GetDetails_Success()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Get;
        var accountId = random.NextLong();
        var campaignId = 4567;
        var requestUri = CampaignUri(campaignId).AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        // Sanity: the campaigns URL must NOT be account-scoped.
        requestUri.Should().NotContain("/api/accounts/");
        requestUri.Should().EndWith($"/api/email_campaigns/{campaignId}");

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .Account(accountId)
            .EmailCampaign(campaignId)
            .GetDetails()
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.Id.Should().Be(campaignId);
        result.Name.Should().Be("Spring Sale");
        result.CurrentState.Should().Be(CampaignState.Draft);
        result.AudienceDefined.Should().BeFalse();
        result.Template.Should().NotBeNull();
        result.Template!.Id.Should().Be(789);
    }

    [Test]
    public async Task Create_Success()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Post;
        var accountId = random.NextLong();
        var requestUri = CollectionUri.AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        // Sanity: the campaigns URL must NOT be account-scoped.
        requestUri.Should().NotContain("/api/accounts/");
        requestUri.Should().EndWith("/api/email_campaigns");

        var request = new CreateEmailCampaignRequest
        {
            Name = "Spring Sale",
            MailsendDomainId = 123,
            FromDisplayName = "Acme Marketing",
            FromLocalPart = "news",
            ReplyTo = new ReplyTo
            {
                DisplayName = "Acme Support",
                LocalPart = "support",
                Domain = "acme.com"
            },
            TemplateAttributes = new EmailCampaignTemplateAttributes
            {
                Subject = "Spring is here — 30% off"
            }
        };

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            // Body must be wrapped under the "email_campaign" envelope.
            .WithJsonContent(request.ToDto(), clientConfig.ToJsonSerializerOptions())
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .Account(accountId)
            .EmailCampaigns()
            .Create(request)
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.Id.Should().Be(4567);
        result.CurrentState.Should().Be(CampaignState.Draft);
    }

    [Test]
    public async Task Create_Unprocessable()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Post;
        var accountId = random.NextLong();
        var requestUri = CollectionUri.AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        var request = new CreateEmailCampaignRequest
        {
            Name = "Spring Sale",
            MailsendDomainId = 123
        };

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .WithJsonContent(request.ToDto(), clientConfig.ToJsonSerializerOptions())
            .Respond(HttpStatusCode.UnprocessableContent, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        var act = () => client
            .Account(accountId)
            .EmailCampaigns()
            .Create(request);


        // Assert
        await act.Should()
            .ThrowAsync<HttpRequestFailedException>()
            .WithMessage("*can't be blank*");

        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Test]
    public async Task Create_ShouldThrowValidationException_WhenNameIsMissing()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Post;
        var accountId = random.NextLong();
        var requestUri = CollectionUri.AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        var request = new CreateEmailCampaignRequest
        {
            MailsendDomainId = 123
        };

        using var mockHttp = new MockHttpMessageHandler();
        var mockedRequest = mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .Respond(HttpStatusCode.OK);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        var act = () => client
            .Account(accountId)
            .EmailCampaigns()
            .Create(request);


        // Assert
        await act.Should().ThrowAsync<RequestValidationException>();

        // No request must be sent when client-side validation fails.
        mockHttp.GetMatchCount(mockedRequest).Should().Be(0);
    }

    [Test]
    public async Task Update_Success()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Patch;
        var accountId = random.NextLong();
        var campaignId = 4567;
        var requestUri = CampaignUri(campaignId).AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        // Sanity: the campaigns URL must NOT be account-scoped.
        requestUri.Should().NotContain("/api/accounts/");

        var scheduledFor = new DateTimeOffset(2026, 6, 1, 9, 0, 0, TimeSpan.Zero);
        var request = new UpdateEmailCampaignRequest
        {
            Name = "Spring Sale (updated)",
            DeliveryMode = DeliveryMode.Scheduled,
            ScheduledFor = scheduledFor,
            DeliveryOptions = new EmailCampaignDeliveryOptions { EmailsPerHour = 1000 },
            TemplateAttributes = new EmailCampaignTemplateAttributes { Id = 789, Subject = "New subject" }
        };

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            // Body must be wrapped under the "email_campaign" envelope.
            .WithJsonContent(request.ToDto(), clientConfig.ToJsonSerializerOptions())
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .Account(accountId)
            .EmailCampaign(campaignId)
            .Update(request)
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.Name.Should().Be("Spring Sale (updated)");
        result.CurrentState.Should().Be(CampaignState.Scheduled);
        result.DeliveryMode.Should().Be(DeliveryMode.Scheduled);
        result.ScheduledFor.Should().Be(scheduledFor);
        result.DeliveryOptions.Should().NotBeNull();
        result.DeliveryOptions!.EmailsPerHour.Should().Be(1000);
    }

    [Test]
    public async Task Update_SerializesScheduledForAsIso8601()
    {
        // Arrange
        var clientConfig = new MailtrapClientOptions(TestContext.CurrentContext.Random.GetString());
        var scheduledFor = new DateTimeOffset(2026, 6, 1, 9, 0, 0, TimeSpan.Zero);

        var request = new UpdateEmailCampaignRequest
        {
            DeliveryMode = DeliveryMode.Scheduled,
            ScheduledFor = scheduledFor
        };


        // Act
        var json = JsonSerializer.Serialize(request.ToDto(), clientConfig.ToJsonSerializerOptions());


        // Assert
        // System.Text.Json (via the SDK's NullableDateTimeOffsetConverter) must emit an
        // ISO-8601 string, NOT ticks or a Unix epoch number.
        json.Should().Contain("\"scheduled_for\":\"2026-06-01T09:00:00");
        json.Should().NotContain("\"scheduled_for\":637");   // not a .NET ticks number
        json.Should().Contain("\"delivery_mode\":\"scheduled\"");

        // Wrapped under the "email_campaign" envelope.
        json.Should().StartWith("{\"email_campaign\":{");
    }

    [Test]
    public async Task Delete_Success_ReturnsDeletedCampaign()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Delete;
        var accountId = random.NextLong();
        var campaignId = 4567;
        var requestUri = CampaignUri(campaignId).AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        // Sanity: the campaigns URL must NOT be account-scoped.
        requestUri.Should().NotContain("/api/accounts/");
        requestUri.Should().EndWith($"/api/email_campaigns/{campaignId}");

        // Delete returns HTTP 200 with the deleted entity in the body (NOT 204).
        using var responseContent = await Feature.LoadFileToStringContent(fileName: "Delete_Success");

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .Account(accountId)
            .EmailCampaign(campaignId)
            .Delete()
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.Id.Should().Be(campaignId);
        result.Name.Should().Be("Spring Sale (updated)");
    }

    [Test]
    public async Task GetStats_Success()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Get;
        var accountId = random.NextLong();
        var campaignId = 4567;
        var requestUri = CampaignUri(campaignId)
            .Append(UrlSegmentsTestConstants.StatsSegment)
            .AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        // Sanity: the campaigns URL must NOT be account-scoped.
        requestUri.Should().NotContain("/api/accounts/");
        requestUri.Should().EndWith($"/api/email_campaigns/{campaignId}/stats");

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .Account(accountId)
            .EmailCampaign(campaignId)
            .GetStats()
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.DeliveryCount.Should().Be(1450);
        result.OpenCount.Should().Be(820);
        result.DeliveryRate.Should().BeApproximately(0.9667f, 0.0001f);
    }

    [Test]
    public async Task GetStats_NotFound()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Get;
        var accountId = random.NextLong();
        var campaignId = random.NextLong();
        var requestUri = CampaignUri(campaignId)
            .Append(UrlSegmentsTestConstants.StatsSegment)
            .AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .Respond(HttpStatusCode.NotFound, MimeTypes.Application.Json, "{\"error\":\"Not Found\"}");

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        var act = () => client
            .Account(accountId)
            .EmailCampaign(campaignId)
            .GetStats();


        // Assert
        await act.Should()
            .ThrowAsync<HttpRequestFailedException>()
            .WithMessage("*Not Found*");

        mockHttp.VerifyNoOutstandingExpectation();
    }
}
