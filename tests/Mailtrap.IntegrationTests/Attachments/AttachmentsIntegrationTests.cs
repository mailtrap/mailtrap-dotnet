namespace Mailtrap.IntegrationTests.Attachments;


[TestFixture]
internal sealed class AttachmentsIntegrationTests
{
    private const string Feature = "Attachments";
    private const string AttachmentTypeQueryParameter = "attachment_type";


    private readonly long _accountId;
    private readonly long _sandboxId;
    private readonly long _messageId;
    private readonly Uri _resourceUri;
    private readonly MailtrapClientOptions _clientConfig;


    public AttachmentsIntegrationTests()
    {
        var random = TestContext.CurrentContext.Random;

        _accountId = random.NextLong();
        _sandboxId = random.NextLong();
        _messageId = random.NextLong();
        _resourceUri = EndpointsTestConstants.ApiDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.AccountsSegment)
            .Append(_accountId)
            .Append(UrlSegmentsTestConstants.InboxesSegment)
            .Append(_sandboxId)
            .Append(UrlSegmentsTestConstants.EmailsSegment)
            .Append(_messageId)
            .Append(UrlSegmentsTestConstants.AttachmentsSegment);

        var token = random.GetString();
        _clientConfig = new MailtrapClientOptions(token);
    }


    [Test]
    public async Task Fetch_WithoutFilter_Success()
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
            .WithExactQueryString("")
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
            .Sandbox(_sandboxId)
            .Message(_messageId)
            .Attachments()
            .Fetch()
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should()
            .NotBeNull().And
            .ContainSingle()
        .Which.Should()
            .Match<EmailAttachment>(a => a.Id == 42);
    }

    [Test]
    public async Task Fetch_WithFilter_Success()
    {
        // Arrange
        var httpMethod = HttpMethod.Get;
        var requestUri = _resourceUri.AbsoluteUri;

        var disposition = DispositionType.Inline;
        var filter = new EmailAttachmentFilter
        {
            Disposition = disposition
        };

        using var responseContent = await Feature.LoadFileToStringContent();

        using var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .Expect(httpMethod, requestUri)
            .WithHeaders("Authorization", $"Bearer {_clientConfig.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .WithQueryString(AttachmentTypeQueryParameter, disposition.ToString())
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
            .Sandbox(_sandboxId)
            .Message(_messageId)
            .Attachments()
            .Fetch(filter)
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should()
            .NotBeNull().And
            .ContainSingle()
        .Which.Should()
            .Match<EmailAttachment>(a => a.Id == 42);
    }


    [Test]
    public async Task GetDetails_Success()
    {
        // Arrange
        var httpMethod = HttpMethod.Get;
        var attachmentId = TestContext.CurrentContext.Random.NextLong();
        var requestUri = _resourceUri.Append(attachmentId).AbsoluteUri;

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
            .Sandbox(_sandboxId)
            .Message(_messageId)
            .Attachment(attachmentId)
            .GetDetails()
            .ConfigureAwait(false);


        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should()
            .NotBeNull().And
            .Match<EmailAttachment>(a => a.Id == 42);
    }
}
