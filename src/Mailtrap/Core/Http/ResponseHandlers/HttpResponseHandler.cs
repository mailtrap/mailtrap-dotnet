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
        if (_httpResponseMessage.StatusCode.GetHashCode() >= 500)
        {
            return new HttpRequestFailedException(
                _httpResponseMessage.RequestMessage.RequestUri,
                _httpResponseMessage.RequestMessage.Method,
                _httpResponseMessage.StatusCode,
                _httpResponseMessage.ReasonPhrase);
        }
        else
        {
            var content = await _httpResponseMessage.Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(content))
            {
                return new HttpRequestFailedException(
                    _httpResponseMessage.RequestMessage.RequestUri,
                    _httpResponseMessage.RequestMessage.Method,
                    _httpResponseMessage.StatusCode,
                    _httpResponseMessage.ReasonPhrase);
            }

            var problem = JsonSerializer.Deserialize<Problem>(content, _jsonSerializerOptions);
            if (problem is null)
            {
                return new HttpRequestFailedException(
                    _httpResponseMessage.RequestMessage.RequestUri,
                    _httpResponseMessage.RequestMessage.Method,
                    _httpResponseMessage.StatusCode,
                    _httpResponseMessage.ReasonPhrase);
            }

            return new HttpRequestFailedException(
                _httpResponseMessage.RequestMessage.RequestUri,
                _httpResponseMessage.RequestMessage.Method,
                _httpResponseMessage.StatusCode,
                _httpResponseMessage.ReasonPhrase,
                problem.ToString());
        }
    }
}
