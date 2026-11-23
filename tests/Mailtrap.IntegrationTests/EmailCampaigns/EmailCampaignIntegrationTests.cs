namespace Mailtrap.IntegrationTests.EmailCampaigns;


[TestFixture]
internal sealed class EmailCampaignIntegrationTests
{
    private const string Feature = "EmailCampaigns";

    private const string TokenQueryParameter = "token";
    private const string PerPageQueryParameter = "per_page";
    private const string SearchQueryParameter = "search";
    private const string StartDateQueryParameter = "start_date";
    private const string EndDateQueryParameter = "end_date";

    private const long SendingDomainId = 4321;


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
        var requestUri = CollectionUri.AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

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
            .EmailCampaigns()
            .GetAll()
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();

        var campaign = result.Data.Should().ContainSingle().Which;
        campaign.Id.Should().Be(4567);
        campaign.DomainId.Should().Be(SendingDomainId);
        campaign.DomainName.Should().Be("acme.com");
        campaign.Name.Should().Be("Spring Sale");
        campaign.CurrentState.Should().Be(CampaignState.Draft);
        campaign.DeliveryMode.Should().Be(DeliveryMode.Rapid);
        campaign.RecipientTotalCount.Should().BeNull();
        campaign.ContactListIds.Should().Equal(55, 56);
        campaign.ContactSegmentIds.Should().Equal(12);

        // The list endpoint omits template bodies.
        campaign.Template.Should().NotBeNull();
        campaign.Template!.BodyHtml.Should().BeNull();

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
        var campaignId = 4567;
        var requestUri = CampaignUri(campaignId).AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

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
            .EmailCampaign(campaignId)
            .GetDetails()
            .ConfigureAwait(false);


        // Assert - the single campaign must be unwrapped from the "data" envelope.
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.Id.Should().Be(campaignId);
        result.DomainId.Should().Be(SendingDomainId);
        result.DomainName.Should().Be("acme.com");
        result.Name.Should().Be("Spring Sale");
        result.CurrentState.Should().Be(CampaignState.Draft);
        result.RecipientTotalCount.Should().BeNull();
        result.ContactListIds.Should().Equal(55, 56);
        result.ContactSegmentIds.Should().Equal(12);

        result.Template.Should().NotBeNull();
        result.Template!.Id.Should().Be(789);
        result.Template.MergeTags.Should().Equal("first_name");
        result.Template.BodyHtml.Should().Contain("__unsubscribe_url__");
        result.Template.BodyText.Should().BeNull();
    }

    [Test]
    public async Task Create_Success()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Post;
        var requestUri = CollectionUri.AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        var request = new CreateEmailCampaignRequest
        {
            Name = "Spring Sale",
            DomainId = SendingDomainId,
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
            },
            ContactListIds = [55, 56],
            ContactSegmentIds = [12]
        };

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            // Body must be flat - no "email_campaign" envelope.
            .WithJsonContent(request, clientConfig.ToJsonSerializerOptions())
            .Respond(HttpStatusCode.Created, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .EmailCampaigns()
            .Create(request)
            .ConfigureAwait(false);


        // Assert - the created campaign must be unwrapped from the "data" envelope.
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.Id.Should().Be(4567);
        result.CurrentState.Should().Be(CampaignState.Draft);
        result.DomainId.Should().Be(SendingDomainId);
    }

    [Test]
    public async Task Create_Unprocessable()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Post;
        var requestUri = CollectionUri.AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        var request = new CreateEmailCampaignRequest
        {
            Name = "Spring Sale",
            DomainId = SendingDomainId,
            FromLocalPart = "news",
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
            .WithJsonContent(request, clientConfig.ToJsonSerializerOptions())
            .Respond(HttpStatusCode.UnprocessableContent, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        var act = () => client
            .EmailCampaigns()
            .Create(request);


        // Assert
        await act.Should()
            .ThrowAsync<HttpRequestFailedException>()
            .WithMessage("*must exist*");

        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Test]
    public async Task Create_ShouldThrowValidationException_WhenNameIsMissing()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Post;
        var requestUri = CollectionUri.AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        var request = new CreateEmailCampaignRequest
        {
            DomainId = SendingDomainId,
            FromLocalPart = "news",
            TemplateAttributes = new EmailCampaignTemplateAttributes
            {
                Subject = "Spring is here — 30% off"
            }
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
        var campaignId = 4567;
        var requestUri = CampaignUri(campaignId).AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        var request = new UpdateEmailCampaignRequest
        {
            Name = "Spring Sale (updated)",
            DeliveryMode = DeliveryMode.Gradual,
            DeliveryOptions = new EmailCampaignDeliveryOptions { EmailsPerHour = 1000 },
            TemplateAttributes = new EmailCampaignTemplateAttributes
            {
                Subject = "New subject",
                BodyHtml = "<html><body><h1>Hi {{first_name}}!</h1><p><a href=\"__unsubscribe_url__\">Unsubscribe</a></p></body></html>",
                MergeTags = ["first_name"]
            },
            ContactListIds = [55],
            ContactSegmentIds = []
        };

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            // Body must be flat - no "email_campaign" envelope.
            .WithJsonContent(request, clientConfig.ToJsonSerializerOptions())
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .EmailCampaign(campaignId)
            .Update(request)
            .ConfigureAwait(false);


        // Assert - the updated campaign must be unwrapped from the "data" envelope.
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.Name.Should().Be("Spring Sale (updated)");
        result.CurrentState.Should().Be(CampaignState.Draft);
        result.DeliveryMode.Should().Be(DeliveryMode.Gradual);
        result.DeliveryOptions.Should().NotBeNull();
        result.DeliveryOptions!.EmailsPerHour.Should().Be(1000);
        result.ContactListIds.Should().Equal(55);
        result.ContactSegmentIds.Should().BeEmpty();
        result.Template.Should().NotBeNull();
        result.Template!.Subject.Should().Be("New subject");
    }

    [Test]
    public async Task Delete_Success()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Delete;
        var campaignId = 4567;
        var requestUri = CampaignUri(campaignId).AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            // Delete returns HTTP 204 with no response body.
            .Respond(HttpStatusCode.NoContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        await client
            .EmailCampaign(campaignId)
            .Delete()
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Test]
    public async Task Start_Success()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Post;
        var campaignId = 4567;
        var requestUri = CampaignUri(campaignId)
            .Append(UrlSegmentsTestConstants.StartSegment)
            .AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        requestUri.Should().EndWith($"/api/email_campaigns/{campaignId}/start");

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
            .EmailCampaign(campaignId)
            .Start()
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.CurrentState.Should().Be(CampaignState.Started);
        result.RecipientTotalCount.Should().Be(1500);
        result.LastStartedAt.Should().NotBeNull();
    }

    [Test]
    public async Task Start_Unprocessable()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Post;
        var campaignId = 4567;
        var requestUri = CampaignUri(campaignId)
            .Append(UrlSegmentsTestConstants.StartSegment)
            .AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .Respond(HttpStatusCode.UnprocessableContent, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        var act = () => client
            .EmailCampaign(campaignId)
            .Start();


        // Assert - a lifecycle 422 carries a single "errors" string.
        await act.Should()
            .ThrowAsync<HttpRequestFailedException>()
            .WithMessage("*Cannot transition*");

        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Test]
    public async Task Start_UnprocessableErrorList()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Post;
        var campaignId = 4567;
        var requestUri = CampaignUri(campaignId)
            .Append(UrlSegmentsTestConstants.StartSegment)
            .AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .Respond(HttpStatusCode.UnprocessableContent, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        var act = () => client
            .EmailCampaign(campaignId)
            .Start();


        // Assert - a sending-validation 422 carries "errors" as a list of strings.
        await act.Should()
            .ThrowAsync<HttpRequestFailedException>()
            .WithMessage("*Campaign design can't be blank*");

        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Test]
    public async Task Schedule_Success()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Post;
        var campaignId = 4567;
        var requestUri = CampaignUri(campaignId)
            .Append(UrlSegmentsTestConstants.ScheduleSegment)
            .AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        requestUri.Should().EndWith($"/api/email_campaigns/{campaignId}/schedule");

        var request = new ScheduleEmailCampaignRequest(DateTimeOffset.UtcNow.AddDays(7));

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            // Body is the bare {"datetime": "<ISO 8601>"} object.
            .WithJsonContent(request, clientConfig.ToJsonSerializerOptions())
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .EmailCampaign(campaignId)
            .Schedule(request)
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.CurrentState.Should().Be(CampaignState.Scheduled);
        result.CurrentStateMetadata.Should().NotBeNull();
        result.CurrentStateMetadata!.ScheduledAt.Should().Be(new DateTimeOffset(2026, 6, 1, 9, 0, 0, TimeSpan.Zero));
    }

    [Test]
    public async Task Schedule_ShouldThrowValidationException_WhenDatetimeIsInPast()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Post;
        var campaignId = 4567;
        var requestUri = CampaignUri(campaignId)
            .Append(UrlSegmentsTestConstants.ScheduleSegment)
            .AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        var request = new ScheduleEmailCampaignRequest(DateTimeOffset.UtcNow.AddDays(-1));

        using var mockHttp = new MockHttpMessageHandler();
        var mockedRequest = mockHttp
            .Expect(httpMethod, requestUri)
            .Respond(HttpStatusCode.OK);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        var act = () => client
            .EmailCampaign(campaignId)
            .Schedule(request);


        // Assert
        await act.Should().ThrowAsync<RequestValidationException>();

        // No request must be sent when client-side validation fails.
        mockHttp.GetMatchCount(mockedRequest).Should().Be(0);
    }

    [Test]
    public async Task Cancel_Success()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Post;
        var campaignId = 4567;
        var requestUri = CampaignUri(campaignId)
            .Append(UrlSegmentsTestConstants.CancelSegment)
            .AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        requestUri.Should().EndWith($"/api/email_campaigns/{campaignId}/cancel");

        // Cancelling returns the campaign back in the "draft" state.
        using var responseContent = await Feature.LoadFileToStringContent(fileName: "GetDetails_Success");

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
            .EmailCampaign(campaignId)
            .Cancel()
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.CurrentState.Should().Be(CampaignState.Draft);
    }

    [Test]
    public async Task Terminate_Success()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Post;
        var campaignId = 4567;
        var requestUri = CampaignUri(campaignId)
            .Append(UrlSegmentsTestConstants.TerminateSegment)
            .AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        requestUri.Should().EndWith($"/api/email_campaigns/{campaignId}/terminate");

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
            .EmailCampaign(campaignId)
            .Terminate()
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.CurrentState.Should().Be(CampaignState.Terminating);
        result.CurrentStateMetadata.Should().NotBeNull();
        result.CurrentStateMetadata!.Reason.Should().Be("Terminated by user");
    }

    [Test]
    public async Task Reset_Success()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Post;
        var campaignId = 4567;
        var requestUri = CampaignUri(campaignId)
            .Append(UrlSegmentsTestConstants.ResetSegment)
            .AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        requestUri.Should().EndWith($"/api/email_campaigns/{campaignId}/reset");

        // Resetting returns the campaign back in the "draft" state.
        using var responseContent = await Feature.LoadFileToStringContent(fileName: "GetDetails_Success");

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
            .EmailCampaign(campaignId)
            .Reset()
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.CurrentState.Should().Be(CampaignState.Draft);
    }

    [Test]
    public async Task GetStats_Success()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Get;
        var campaignId = 4567;
        var requestUri = CampaignUri(campaignId)
            .Append(UrlSegmentsTestConstants.StatsSegment)
            .AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

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
            .EmailCampaign(campaignId)
            .GetStats()
            .ConfigureAwait(false);


        // Assert - the stats must be unwrapped from the "data" envelope.
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.DeliveryCount.Should().Be(1450);
        result.OpenCount.Should().Be(820);
        result.DeliveryRate.Should().BeApproximately(0.9667f, 0.0001f);
    }

    [Test]
    public async Task GetStats_WithFilter_SerializesStartAndEndDate()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Get;
        var campaignId = 4567;
        var requestUri = CampaignUri(campaignId)
            .Append(UrlSegmentsTestConstants.StatsSegment)
            .AbsoluteUri;
        var token = random.GetString();
        var clientConfig = new MailtrapClientOptions(token);

        var startDate = "2026-05-01";
        var endDate = "2026-05-31";

        var filter = new EmailCampaignStatsFilter
        {
            StartDate = startDate,
            EndDate = endDate
        };

        using var responseContent = await Feature.LoadFileToStringContent(fileName: "GetStats_Success");

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .WithQueryString(StartDateQueryParameter, startDate)
            .WithQueryString(EndDateQueryParameter, endDate)
            .Respond(HttpStatusCode.OK, responseContent);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientConfig)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        using var services = serviceCollection.BuildServiceProvider();

        var client = services.GetRequiredService<IMailtrapClient>();


        // Act
        var result = await client
            .EmailCampaign(campaignId)
            .GetStats(filter)
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.SentCount.Should().Be(1500);
    }

    [Test]
    public async Task GetStats_NotFound()
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var httpMethod = HttpMethod.Get;
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
            .EmailCampaign(campaignId)
            .GetStats();


        // Assert
        await act.Should()
            .ThrowAsync<HttpRequestFailedException>()
            .WithMessage("*Not Found*");

        mockHttp.VerifyNoOutstandingExpectation();
    }
}
