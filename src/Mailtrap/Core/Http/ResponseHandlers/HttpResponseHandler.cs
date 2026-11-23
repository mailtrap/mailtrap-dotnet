namespace Mailtrap.Core.Http.ResponseHandlers;


internal abstract class HttpResponseHandler<T> : IHttpResponseHandler<T>
{
    protected readonly JsonSerializerOptions _jsonSerializerOptions;
    protected readonly HttpResponseMessage _httpResponseMessage;


    protected HttpResponseHandler(
        JsonSerializerOptions jsonSerializerOptions,
        HttpResponseMessage httpResponseMessage)
    {
        Ensure.NotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));
        Ensure.NotNull(httpResponseMessage, nameof(httpResponseMessage));

        _jsonSerializerOptions = jsonSerializerOptions;
        _httpResponseMessage = httpResponseMessage;
    }


    public async Task<T> ProcessResponse(CancellationToken cancellationToken = default)
    {
        if (_httpResponseMessage.IsSuccessStatusCode)
        {
            return await ProcessSuccessResponse(cancellationToken).ConfigureAwait(false);
        }
        else
        {
            var ex = await ProcessFailureResponse().ConfigureAwait(false);

            throw ex;
        }
    }


    protected abstract Task<T> ProcessSuccessResponse(CancellationToken cancellationToken);

    protected virtual async Task<MailtrapApiException> ProcessFailureResponse()
    {
        var content = _httpResponseMessage.Content is null
            ? null
            : await _httpResponseMessage.Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

        var problem = string.IsNullOrWhiteSpace(content)
            ? null
            : _httpResponseMessage.StatusCode.GetHashCode() >= 500
                ? TryDeserializeProblem(content!)
                : JsonSerializer.Deserialize<Problem>(content!, _jsonSerializerOptions);

        return new HttpRequestFailedException(
            _httpResponseMessage.RequestMessage.RequestUri,
            _httpResponseMessage.RequestMessage.Method,
            _httpResponseMessage.StatusCode,
            _httpResponseMessage.ReasonPhrase,
            problem?.ToString());
    }


    // A 5xx body may come from a proxy instead of the API, such as an HTML error page,
    // so its error details are read only when it is JSON.
    private Problem? TryDeserializeProblem(string content)
    {
        try
        {
            return JsonSerializer.Deserialize<Problem>(content, _jsonSerializerOptions);
        }
        catch (JsonException)
        {
            return null;
        }
    }
}
