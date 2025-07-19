﻿namespace Mailtrap.Core.Rest.Commands;


internal abstract class RestResourceCommandWithRequest<TRequest, TResponse>
    : RestResourceCommand<TResponse>
    where TRequest : class
{
    protected readonly TRequest _request;


    public RestResourceCommandWithRequest(
        IHttpClientProvider httpClientProvider,
        IHttpRequestMessageFactory httpRequestMessageFactory,
        IHttpResponseHandlerFactory httpResponseHandlerFactory,
        Uri resourceUri,
        HttpMethod httpMethod,
        TRequest request)
        : base(
            httpClientProvider,
            httpRequestMessageFactory,
            httpResponseHandlerFactory,
            resourceUri,
            httpMethod)
    {
        ValidateRequest(request);

        _request = request;
    }


    protected override HttpRequestMessage CreateHttpRequest()
        => _httpRequestMessageFactory.CreateWithContent(HttpMethod, ResourceUri, _request);


    private void ValidateRequest(TRequest request)
    {
        Ensure.NotNull(request, nameof(request));

        if (request is not IValidatable validatable)
        {
            return;
        }

        var validationResult = validatable.Validate();

        if (validationResult.IsValid)
        {
            return;
        }

        throw new RequestValidationException(ResourceUri, HttpMethod, validationResult);
    }
}
