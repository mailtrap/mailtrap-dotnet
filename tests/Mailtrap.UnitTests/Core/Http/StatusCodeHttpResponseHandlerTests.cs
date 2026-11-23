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
