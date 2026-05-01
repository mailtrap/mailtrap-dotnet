namespace Mailtrap.Core.Rest.Commands;


internal sealed class PostWithoutBodyRestResourceCommand<TResponse> : RestResourceCommand<TResponse>
{
    public PostWithoutBodyRestResourceCommand(
        IHttpClientProvider httpClientProvider,
        IHttpRequestMessageFactory httpRequestMessageFactory,
        IHttpResponseHandlerFactory httpResponseHandlerFactory,
        Uri resourceUri)
        : base(
            httpClientProvider,
            httpRequestMessageFactory,
            httpResponseHandlerFactory,
            resourceUri,
            HttpMethod.Post)
    { }
}
