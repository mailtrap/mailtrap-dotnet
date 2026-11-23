namespace Mailtrap.UnitTests.Core.Http;


[TestFixture]
internal sealed class StatusCodeHttpResponseHandlerTests
{
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.BadGateway)]
    public async Task ProcessResponse_ShouldThrowException_WhenHttpCodeIs5xx(HttpStatusCode statusCode)
    {
        // Arrange
        using var httpMessage = new HttpResponseMessage(statusCode)
        {
            RequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://test.com")
        };

        var handler = new StatusCodeHttpResponseHandler(
            MailtrapClientOptions.Default.ToJsonSerializerOptions(), httpMessage);

        var act = () => handler.ProcessResponse();


        // Assert
        await act.Should()
            .ThrowAsync<HttpRequestFailedException>()
            .Where(ex => ex.StatusCode == statusCode);
    }

    [Test]
    public async Task ProcessResponse_ShouldThrowExceptionWithErrors_WhenHttpCodeIs5xxWithJsonBody()
    {
        // Arrange
        var statusCode = HttpStatusCode.ServiceUnavailable;

        using var httpMessage = new HttpResponseMessage(statusCode)
        {
            RequestMessage = new HttpRequestMessage(HttpMethod.Post, "http://test.com"),
            Content = new StringContent(
                "{\"errors\":\"The contact was saved, but its subscription topic preferences were not.\"}",
                Encoding.UTF8,
                MimeTypes.Application.Json)
        };

        var handler = new StatusCodeHttpResponseHandler(
            MailtrapClientOptions.Default.ToJsonSerializerOptions(), httpMessage);

        var act = () => handler.ProcessResponse();


        // Assert
        await act.Should()
            .ThrowAsync<HttpRequestFailedException>()
            .Where(ex => ex.StatusCode == statusCode)
            .WithMessage("*The contact was saved, but its subscription topic preferences were not.*");
    }

    [Test]
    public async Task ProcessResponse_ShouldThrowExceptionWithoutDetails_WhenHttpCodeIs5xxWithNonJsonBody()
    {
        // Arrange - a proxy's HTML error page must not turn into a JSON parsing error.
        var statusCode = HttpStatusCode.BadGateway;

        using var httpMessage = new HttpResponseMessage(statusCode)
        {
            RequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://test.com"),
            Content = new StringContent("<html><body>502 Bad Gateway</body></html>", Encoding.UTF8, "text/html")
        };

        var handler = new StatusCodeHttpResponseHandler(
            MailtrapClientOptions.Default.ToJsonSerializerOptions(), httpMessage);

        var act = () => handler.ProcessResponse();


        // Assert
        var exception = await act.Should()
            .ThrowAsync<HttpRequestFailedException>()
            .Where(ex => ex.StatusCode == statusCode);

        exception.Which.Message.Should().NotContain("Error details");
    }

    [Test]
    public async Task ProcessResponse_ShouldThrowExceptionWithDetails_WhenHttpCodeIs4xx()
    {
        // Arrange
        var statusCode = HttpStatusCode.NotFound;

        using var httpMessage = new HttpResponseMessage(statusCode)
        {
            RequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://test.com")
        };

        var handler = new StatusCodeHttpResponseHandler(
            MailtrapClientOptions.Default.ToJsonSerializerOptions(), httpMessage);

        var act = () => handler.ProcessResponse();


        // Assert
        await act.Should()
            .ThrowAsync<HttpRequestFailedException>()
            .Where(ex => ex.StatusCode == statusCode);
    }

    [Test]
    public async Task ProcessResponse_ShouldThrowEmptyResponseException_WhenResponseContentIsNull()
    {
        // Arrange
        var statusCode = HttpStatusCode.Forbidden;
        var httpMethod = HttpMethod.Get;

        using var httpMessage = new HttpResponseMessage(statusCode)
        {
            RequestMessage = new HttpRequestMessage(httpMethod, "http://test.com"),
            Content = new StringContent("null")
        };

        var handler = new StatusCodeHttpResponseHandler(
            MailtrapClientOptions.Default.ToJsonSerializerOptions(), httpMessage);

        var act = () => handler.ProcessResponse();


        // Assert
        await act.Should()
            .ThrowAsync<HttpRequestFailedException>()
            .Where(ex => ex.StatusCode == statusCode);
    }
}
