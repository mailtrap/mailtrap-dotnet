namespace Mailtrap.IntegrationTests.Emails;


[TestFixture]
internal sealed class BatchEmailIntegrationTests
{
    internal sealed record BatchEmailTestCase(MailtrapClientOptions Config, string SendUri);


    [TestCaseSource(nameof(TestCasesForDefault))]
    public async Task BatchEmail_Should_RouteToProperUrl_WhenDefaultClientIsUsed(BatchEmailTestCase testCase)
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;
        var request = CreateValidRequest();
        using var mockHttp = new MockHttpMessageHandler();

        var response = BatchEmailResponse.CreateSuccess(
            BatchSendEmailResponse.CreateSuccess(random.NextGuid().ToString()),
            BatchSendEmailResponse.CreateSuccess(random.NextGuid().ToString()));
        using var responseContent = JsonContent.Create(response);

        using var services = CreateServiceProvider(testCase.Config, testCase.SendUri, request, mockHttp, responseContent);

        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var result = await client
            .BatchEmail()
            .Send(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should()
            .NotBeNull().And
            .BeEquivalentTo(response);

        result.Success.Should().BeTrue();
        result.Responses.Should().HaveCount(2);
    }

    [TestCaseSource(nameof(TestCasesForDefault))]
    public async Task BatchEmail_Should_RouteToProperUrl_WhenEmailClientIsUsed(BatchEmailTestCase testCase)
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;
        var request = CreateValidRequest();
        using var mockHttp = new MockHttpMessageHandler();

        var response = BatchEmailResponse.CreateSuccess(
            BatchSendEmailResponse.CreateSuccess(random.NextGuid().ToString()),
            BatchSendEmailResponse.CreateSuccess(random.NextGuid().ToString()));
        using var responseContent = JsonContent.Create(response);

        using var services = CreateServiceProvider(testCase.Config, testCase.SendUri, request, mockHttp, responseContent);

        var client = services.GetRequiredService<IBatchEmailClient>();

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
        result!.Responses.Should().HaveCount(2);
    }

    [TestCaseSource(nameof(TestCasesForNonDefault))]
    public async Task BatchEmail_Should_RouteToProperUrl_WhenTransactionalClientIsUsed(MailtrapClientOptions config)
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;
        var request = CreateValidRequest();
        using var mockHttp = new MockHttpMessageHandler();

        var response = BatchEmailResponse.CreateSuccess(
            BatchSendEmailResponse.CreateSuccess(random.NextGuid().ToString()),
            BatchSendEmailResponse.CreateSuccess(random.NextGuid().ToString()));
        using var responseContent = JsonContent.Create(response);

        var sendUri = EndpointsTestConstants.SendDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.BatchEmailSegment)
            .AbsoluteUri;
        using var services = CreateServiceProvider(config, sendUri, request, mockHttp, responseContent);

        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var result = await client
            .BatchTransactional()
            .Send(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should()
            .NotBeNull().And
            .BeEquivalentTo(response);

        result!.Success.Should().BeTrue();
        result!.Responses.Should().HaveCount(2);
    }

    [TestCaseSource(nameof(TestCasesForNonDefault))]
    public async Task BatchEmail_Should_RouteToProperUrl_WhenBulkClientIsUsed(MailtrapClientOptions config)
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;
        var request = CreateValidRequest();
        using var mockHttp = new MockHttpMessageHandler();

        var response = BatchEmailResponse.CreateSuccess(
            BatchSendEmailResponse.CreateSuccess(random.NextGuid().ToString()),
            BatchSendEmailResponse.CreateSuccess(random.NextGuid().ToString()));
        using var responseContent = JsonContent.Create(response);

        var sendUri = EndpointsTestConstants.BulkDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.BatchEmailSegment)
            .AbsoluteUri;
        using var services = CreateServiceProvider(config, sendUri, request, mockHttp, responseContent);

        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var result = await client
            .BatchBulk()
            .Send(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should()
            .NotBeNull().And
            .BeEquivalentTo(response);

        result!.Success.Should().BeTrue();
        result!.Responses.Should().HaveCount(2);
    }

    [TestCaseSource(nameof(TestCasesForNonDefault))]
    public async Task BatchEmail_Should_RouteToProperUrl_WhenTestClientIsUsed(MailtrapClientOptions config)
    {
        // Arrange
        var random = TestContext.CurrentContext.Random;

        var request = CreateValidRequest();
        using var mockHttp = new MockHttpMessageHandler();

        var response = BatchEmailResponse.CreateSuccess(
            BatchSendEmailResponse.CreateSuccess(random.NextGuid().ToString()),
            BatchSendEmailResponse.CreateSuccess(random.NextGuid().ToString()));
        using var responseContent = JsonContent.Create(response);

        var inboxId = random.NextLong();
        var sendUri = EndpointsTestConstants.TestDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.BatchEmailSegment)
            .Append(inboxId)
            .AbsoluteUri;
        using var services = CreateServiceProvider(config, sendUri, request, mockHttp, responseContent);

        var client = services.GetRequiredService<IMailtrapClient>();

        // Act
        var result = await client
            .BatchTest(inboxId)
            .Send(request)
            .ConfigureAwait(false);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();

        result.Should()
            .NotBeNull().And
            .BeEquivalentTo(response);

        result!.Success.Should().BeTrue();
        result!.Responses.Should().HaveCount(2);
    }

    #region Test Cases

    private static IEnumerable<BatchEmailTestCase> TestCasesForDefault()
    {
        var random = TestContext.CurrentContext.Random;
        var token = random.GetString();
        var inboxId = random.NextLong();
        var sendUri = EndpointsTestConstants.SendDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.BatchEmailSegment)
            .AbsoluteUri;
        var testUri = EndpointsTestConstants.TestDefaultUrl
            .Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.BatchEmailSegment)
            .Append(inboxId)
            .AbsoluteUri;
        var bulkUri = EndpointsTestConstants.BulkDefaultUrl.Append(
                UrlSegmentsTestConstants.ApiRootSegment,
                UrlSegmentsTestConstants.BatchEmailSegment)
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

    private static BatchEmailRequest CreateValidRequest()
    {
        var baseRequest = EmailRequest
            .Create()
            .From("john.doe@demomailtrap.com", "John Doe")
            .Subject("Invitation to Earth")
            .Text("Dear Guest,\nIt will be a great pleasure to see you on our blue planet next weekend.\nBest regards, John.");

        var nestedRequest1 = SendEmailRequest
            .Create()
            .To("hero.bill@galaxy.net")
            .Text("Dear Bill,\nIt will be a great pleasure to see you on our blue planet next weekend.\nBest regards, John.");

        var nestedRequest2 = SendEmailRequest
            .Create()
            .To("star.lord@galaxy.net")
            .Text("Dear Peter,\nIt will be a great pleasure to see you on our blue planet next weekend.\nBest regards, John.");

        return new BatchEmailRequest()
        {
            Base = baseRequest,
            Requests =
            {
                nestedRequest1,
                nestedRequest2
            }
        };
    }

    private static ServiceProvider CreateServiceProvider(
        MailtrapClientOptions config,
        string sendUri,
        BatchEmailRequest request,
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
