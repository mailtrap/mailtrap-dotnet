namespace Mailtrap.IntegrationTests.TestExtensions;

/// <summary>
/// Provides helper methods to configure and create mock <see cref="IMailtrapClient"/> instances
/// for integration testing purposes.
/// </summary>
internal static class MockHttpHandlerHelpers
{
    /// <summary>
    /// Configures the mock HTTP handler with query parameters and creates a <see cref="IMailtrapClient"/> instance.
    /// </summary>
    /// <typeparam name="T">The type of the request content used in the mocked request.</typeparam>
    /// <param name="mockHttp">The mock HTTP message handler used to intercept and verify requests.</param>
    /// <param name="httpMethod">The HTTP method to match (e.g., <see cref="HttpMethod.Get"/>).</param>
    /// <param name="requestUri">The request URI to match.</param>
    /// <param name="responseContent">The mock response content to return.</param>
    /// <param name="responseStatusCode">The HTTP status code to return in the response.</param>
    /// <param name="clientOptions">The configuration options for the Mailtrap client.</param>
    /// <param name="queryParameterName">Query parameter name to match. If empty, performs exact match.</param>
    /// <param name="queryParameterValue">Optional query parameter value to match. Ignored if name is <see langword="null"/> or empty.</param>
    /// <param name="content">Optional request content to match against.</param>
    /// <param name="isRequestExpectation">If <see langword="true"/>, configures as request expectation; otherwise, as backend definition.</param>
    /// <returns>The configured <see cref="IMailtrapClient"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if any required argument is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="requestUri"/> is empty or whitespace.</exception>
    internal static IMailtrapClient ConfigureWithQueryAndCreateClient<T>(
        this MockHttpMessageHandler mockHttp,
        HttpMethod httpMethod,
        string requestUri,
        StringContent responseContent,
        HttpStatusCode responseStatusCode,
        MailtrapClientOptions clientOptions,
        string queryParameterName,
        string? queryParameterValue = null,
        T? content = default,
        bool isRequestExpectation = true)
    {
        ArgumentNullException.ThrowIfNull(mockHttp);
        ArgumentNullException.ThrowIfNull(httpMethod);
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri);
        ArgumentNullException.ThrowIfNull(responseContent);
        ArgumentNullException.ThrowIfNull(clientOptions);

        mockHttp
            .ConfigureExpectation(
                isRequestExpectation,
                httpMethod,
                requestUri)
            .ConfigureMockRequest(
                responseContent,
                responseStatusCode,
                clientOptions,
                content,
                queryParameterName,
                queryParameterValue);

        return mockHttp.BuildClient(clientOptions);
    }

    /// <inheritdoc cref="ConfigureWithQueryAndCreateClient{T}(MockHttpMessageHandler,HttpMethod,string,StringContent,HttpStatusCode,MailtrapClientOptions,string,string?,T,bool)" />
    internal static IMailtrapClient ConfigureWithQueryAndCreateClient(
        this MockHttpMessageHandler mockHttp,
        HttpMethod httpMethod,
        string requestUri,
        StringContent responseContent,
        HttpStatusCode responseStatusCode,
        MailtrapClientOptions clientOptions,
        string queryParameterName,
        string? queryParameterValue = null,
        bool isRequestExpectation = true)
        => mockHttp.ConfigureWithQueryAndCreateClient<object>(
            httpMethod,
            requestUri,
            responseContent,
            responseStatusCode,
            clientOptions,
            queryParameterName,
            queryParameterValue,
            null,
            isRequestExpectation);

    /// <summary>
    /// Configures the mock HTTP handler and creates a Mailtrap client.
    /// </summary>
    /// <typeparam name="T">The type of the request content used in the mocked request.</typeparam>
    /// <param name="mockHttp">The mock HTTP message handler used to intercept and verify requests.</param>
    /// <param name="httpMethod">The HTTP method to match (e.g., <see cref="HttpMethod.Post"/>).</param>
    /// <param name="requestUri">The request URI to match.</param>
    /// <param name="responseContent">The mock response content to return.</param>
    /// <param name="responseStatusCode">The HTTP status code to return in the response.</param>
    /// <param name="clientOptions">The Mailtrap client options.</param>
    /// <param name="content">Optional request content to match.</param>
    /// <param name="isRequestExpectation">If <see langword="true"/>, configures as request expectation; otherwise, as backend definition.</param>
    /// <returns>The configured <see cref="IMailtrapClient"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if any required argument is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="requestUri"/> is empty or whitespace.</exception>
    internal static IMailtrapClient ConfigureAndCreateClient<T>(this MockHttpMessageHandler mockHttp,
        HttpMethod httpMethod,
        string requestUri,
        StringContent responseContent,
        HttpStatusCode responseStatusCode,
        MailtrapClientOptions clientOptions,
        T? content = default,
        bool isRequestExpectation = true)
    {
        ArgumentNullException.ThrowIfNull(mockHttp);
        ArgumentNullException.ThrowIfNull(httpMethod);
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri);
        ArgumentNullException.ThrowIfNull(responseContent);
        ArgumentNullException.ThrowIfNull(clientOptions);

        mockHttp
            .ConfigureExpectation(isRequestExpectation, httpMethod, requestUri)
            .ConfigureMockRequest(responseContent, responseStatusCode, clientOptions, content);

        return mockHttp.BuildClient(clientOptions);
    }

    /// <inheritdoc cref="ConfigureAndCreateClient{T}(MockHttpMessageHandler,HttpMethod,string,StringContent,HttpStatusCode,MailtrapClientOptions,T,bool)" />
    internal static IMailtrapClient ConfigureAndCreateClient(
        this MockHttpMessageHandler mockHttp,
        HttpMethod httpMethod,
        string requestUri,
        StringContent responseContent,
        HttpStatusCode responseStatusCode,
        MailtrapClientOptions clientOptions,
        bool isRequestExpectation = true)
        => mockHttp.ConfigureAndCreateClient<object>(
            httpMethod,
            requestUri,
            responseContent,
            responseStatusCode,
            clientOptions,
            null,
            isRequestExpectation);

    /// <summary>
    /// Configures the mock request with headers, optional query parameters, and response definition.
    /// </summary>
    /// <typeparam name="T">The type of the request content.</typeparam>
    /// <param name="mockRequest">The mock request to configure.</param>
    /// <param name="responseContent">The response content to return.</param>
    /// <param name="responseStatusCode">The HTTP status code to return in the response.</param>
    /// <param name="clientOptions">The Mailtrap client options.</param>
    /// <param name="content">Optional request body to match against.</param>
    /// <param name="queryParameterName">
    /// Optional query parameter name to match.
    /// If <see langword="null"/>, query parameter matching is skipped.
    /// If empty string or whitespace, performs exact query string matching.
    /// If non-empty with a non-null value, matches the specific query parameter.
    /// </param>
    /// <param name="queryParameterValue">
    /// Optional query parameter value to match.
    /// Ignored if <paramref name="queryParameterName"/> is <see langword="null"/> or empty.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="mockRequest"/>, <paramref name="responseContent"/>, or <paramref name="clientOptions"/> is <see langword="null"/>.</exception>
    private static void ConfigureMockRequest<T>(this MockedRequest mockRequest,
        StringContent responseContent,
        HttpStatusCode responseStatusCode,
        MailtrapClientOptions clientOptions,
        T? content,
        string? queryParameterName = null,
        string? queryParameterValue = null)
    {
        ArgumentNullException.ThrowIfNull(mockRequest);
        ArgumentNullException.ThrowIfNull(responseContent);
        ArgumentNullException.ThrowIfNull(clientOptions);

        mockRequest
            .WithHeaders("Authorization", $"Bearer {clientOptions.ApiToken}")
            .WithHeaders("Accept", MimeTypes.Application.Json)
            .WithHeaders("User-Agent", HeaderValues.UserAgent.ToString());

        if (content is not null)
        {
            mockRequest.WithJsonContent(content, clientOptions.ToJsonSerializerOptions());
        }

        if (!string.IsNullOrWhiteSpace(queryParameterName) && queryParameterValue is not null)
        {
            mockRequest.WithQueryString(queryParameterName, queryParameterValue);
        }
        else if (queryParameterName is not null && string.IsNullOrWhiteSpace(queryParameterName))
        {
            mockRequest.WithExactQueryString(queryParameterName);
        }

        mockRequest.Respond(responseStatusCode, responseContent);
    }

    /// <summary>
    /// Builds a <see cref="IMailtrapClient"/> instance using the specified mock HTTP handler and client options.
    /// </summary>
    /// <param name="mockHttp">The mock HTTP message handler to inject into the client.</param>
    /// <param name="clientOptions">The Mailtrap client configuration options.</param>
    /// <returns>A configured <see cref="IMailtrapClient"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="mockHttp"/> or <paramref name="clientOptions"/> is <see langword="null"/>.</exception>
    private static IMailtrapClient BuildClient(this MockHttpMessageHandler mockHttp, MailtrapClientOptions clientOptions)
    {
        ArgumentNullException.ThrowIfNull(mockHttp);
        ArgumentNullException.ThrowIfNull(clientOptions);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddMailtrapClient(clientOptions)
            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);

        return serviceCollection
            .BuildServiceProvider()
            .GetRequiredService<IMailtrapClient>();
    }

    /// <summary>
    /// Configures a mock request definition as either a request expectation or a backend handler.
    /// </summary>
    /// <param name="mockHttp">The mock HTTP message handler to configure.</param>
    /// <param name="isRequestExpectation">If <see langword="true"/>, creates a request expectation; otherwise, creates a backend definition.</param>
    /// <param name="httpMethod">The HTTP method to match.</param>
    /// <param name="requestUri">The request URI to match.</param>
    /// <returns>A <see cref="MockedRequest"/> object representing the configured mock request.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="mockHttp"/> or <paramref name="httpMethod"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="requestUri"/> is empty or whitespace.</exception>
    private static MockedRequest ConfigureExpectation(
        this MockHttpMessageHandler mockHttp,
        bool isRequestExpectation,
        HttpMethod httpMethod,
        string requestUri)
    {
        ArgumentNullException.ThrowIfNull(mockHttp);
        ArgumentNullException.ThrowIfNull(httpMethod);
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri);

        return isRequestExpectation
            ? mockHttp.Expect(httpMethod, requestUri)
            : mockHttp.When(httpMethod, requestUri);
    }
}
