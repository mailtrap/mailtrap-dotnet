namespace Mailtrap.IntegrationTests.Emails;


[TestFixture]
internal sealed class SendEmailIntegrationTests
{
    internal sealed record SendEmailTestCase(MailtrapClientOptions Config, string SendUri);


    [TestCaseSource(nameof(TestCasesForDefault))]
    public async Task SendEmail_Should_RouteToProperUrl_WhenDefaultClientIsUsed(SendEmailTestCase testCase)
    {
        // Arrange
        var request = CreateValidRequest();
        using var mockHttp = new MockHttpMessageHandler();

        var messageId = TestContext.CurrentContext.Random.NextGuid().ToString();
        var response = SendEmailResponse.CreateSuccess(messageId);
        using var responseContent = JsonContent.Create(response);

        using var services = CreateServiceProvider(testCase.Config, testCase.SendUri, request, mockHttp, responseContent);

        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var result = await client
            .Email()
            .Send(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should()
            .NotBeNull().And
            .BeEquivalentTo(response);

        result!.Success.Should().BeTrue();
        result!.MessageIds.Should().ContainSingle(m => m == messageId);
    }

    [TestCaseSource(nameof(TestCasesForDefault))]
    public async Task SendEmail_Should_RouteToProperUrl_WhenEmailClientIsUsed(SendEmailTestCase testCase)
    {
        // Arrange
        var request = CreateValidRequest();
        using var mockHttp = new MockHttpMessageHandler();

        var messageId = TestContext.CurrentContext.Random.NextGuid().ToString();
        var response = SendEmailResponse.CreateSuccess(messageId);
        using var responseContent = JsonContent.Create(response);

        using var services = CreateServiceProvider(testCase.Config, testCase.SendUri, request, mockHttp, responseContent);

        var client = services.GetRequiredService<ISendEmailClient>();

        // Act
        var result = await client
            .Send(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should()
            .NotBeNull().And
            .BeEquivalentTo(response);

        result!.Success.Should().BeTrue();
        result!.MessageIds.Should().ContainSingle(m => m == messageId);
    }

    [TestCaseSource(nameof(TestCasesForNonDefault))]
    public async Task SendEmail_Should_RouteToProperUrl_WhenTransactionalClientIsUsed(MailtrapClientOptions config)
    {
        // Arrange
        var request = CreateValidRequest();
        using var mockHttp = new MockHttpMessageHandler();

        var messageId = TestContext.CurrentContext.Random.NextGuid().ToString();
        var response = SendEmailResponse.CreateSuccess(messageId);
        using var responseContent = JsonContent.Create(response);

        var sendUri = EndpointsTestConstants.SendDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.SendEmailSegment)
            .AbsoluteUri;
        using var services = CreateServiceProvider(config, sendUri, request, mockHttp, responseContent);

        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var result = await client
            .Transactional()
            .Send(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should()
            .NotBeNull().And
            .BeEquivalentTo(response);

        result!.Success.Should().BeTrue();
        result!.MessageIds.Should().ContainSingle(m => m == messageId);
    }

    [TestCaseSource(nameof(TestCasesForNonDefault))]
    public async Task SendEmail_Should_RouteToProperUrl_WhenBulkClientIsUsed(MailtrapClientOptions config)
    {
        // Arrange
        var request = CreateValidRequest();
        using var mockHttp = new MockHttpMessageHandler();

        var messageId = TestContext.CurrentContext.Random.NextGuid().ToString();
        var response = SendEmailResponse.CreateSuccess(messageId);
        using var responseContent = JsonContent.Create(response);

        var sendUri = EndpointsTestConstants.BulkDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.SendEmailSegment)
            .AbsoluteUri;
        using var services = CreateServiceProvider(config, sendUri, request, mockHttp, responseContent);

        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var result = await client
            .Bulk()
            .Send(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should()
            .NotBeNull().And
            .BeEquivalentTo(response);

        result!.Success.Should().BeTrue();
        result!.MessageIds.Should().ContainSingle(m => m == messageId);
    }

    [TestCaseSource(nameof(TestCasesForNonDefault))]
    public async Task SendEmail_Should_RouteToProperUrl_WhenTestClientIsUsed(MailtrapClientOptions config)
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var request = CreateValidRequest();
        using var mockHttp = new MockHttpMessageHandler();

        var messageId = random.NextGuid().ToString();
        var response = SendEmailResponse.CreateSuccess(messageId);
        using var responseContent = JsonContent.Create(response);

        var inboxId = random.NextLong();
        var sendUri = EndpointsTestConstants.TestDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.SendEmailSegment)
            .Append(inboxId)
            .AbsoluteUri;
        using var services = CreateServiceProvider(config, sendUri, request, mockHttp, responseContent);

        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var result = await client
            .Test(inboxId)
            .Send(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should()
            .NotBeNull().And
            .BeEquivalentTo(response);

        result!.Success.Should().BeTrue();
        result!.MessageIds.Should().ContainSingle(m => m == messageId);
    }


    #region Test Cases

    private static IEnumerable<SendEmailTestCase> TestCasesForDefault()
    {
        var random = TestContext.CurrentContext.Random;
        var token = random.GetString();
        var inboxId = random.NextLong();
        var sendUri = EndpointsTestConstants.SendDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.SendEmailSegment)
            .AbsoluteUri;
        var testUri = EndpointsTestConstants.TestDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.SendEmailSegment)
            .Append(inboxId)
            .AbsoluteUri;
        var bulkUri = EndpointsTestConstants.BulkDefaultUrl.Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.SendEmailSegment)
            .AbsoluteUri;

        yield return new(
            new MailtrapClientOptions(token),
            sendUri);

        yield return new(
            new MailtrapClientOptions(token) { PrettyJson = true },
            sendUri);

        yield return new(
            new MailtrapClientOptions(token) { UseBulkApi = true },
            bulkUri);

        yield return new(
            new MailtrapClientOptions(token) { InboxId = inboxId },
            testUri);

        yield return new(
            new MailtrapClientOptions(token) { InboxId = inboxId, UseBulkApi = true },
            testUri);
    }

    private static IEnumerable<MailtrapClientOptions> TestCasesForNonDefault()
    {
        var random = TestContext.CurrentContext.Random;
        var token = random.GetString();
        var inboxId = random.NextLong();

        yield return new(token);
        yield return new(token) { PrettyJson = true };
        yield return new(token) { UseBulkApi = true };
        yield return new(token) { InboxId = inboxId };
        yield return new(token) { InboxId = inboxId, UseBulkApi = true };
    }

    #endregion

    #region Setup Helpers

    private static SendEmailRequest CreateValidRequest()
    {
        return SendEmailRequest
            .Create()
            .From("john.doe@demomailtrap.com", "John Doe")
            .To("hero.bill@galaxy.net")
            .Subject("Invitation to Earth")
            .Text("Dear Bill,\nIt will be a great pleasure to see you on our blue planet next weekend.\nBest regards, John.");
    }

    private static ServiceProvider CreateServiceProvider(
        MailtrapClientOptions config,
        string sendUri,
        SendEmailRequest request,
        MockHttpMessageHandler mockHttp,
        JsonContent responseContent)
    {
        var serviceCollection = new ServiceCollection();

        var httpMethod = HttpMethod.Post;
        var jsonSerializerOptions = config.ToJsonSerializerOptions();

        mockHttp
            .Expect(httpMethod, sendUri)
            .WithJsonContent(request, jsonSerializerOptions)
            .WithHeaders("Authorization", $"Bearer {config.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString())
            .With(r => r.Content?.Headers.ContentType?.MediaType == MimeTypes.Application.Json)
            .Respond(HttpStatusCode.OK, responseContent);

        serviceCollection
            .AddMailtrapClient(config)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        return serviceCollection.BuildServiceProvider();
    }

    #endregion
}
