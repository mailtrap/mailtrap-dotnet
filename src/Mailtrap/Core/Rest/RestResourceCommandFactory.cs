namespace Mailtrap.Core.Rest;


internal sealed class RestResourceCommandFactory : IRestResourceCommandFactory
{
    private readonly IHttpClientProvider _httpClientProvider;
    private readonly IHttpResponseHandlerFactory _httpResponseHandlerFactory;
    private readonly IHttpRequestMessageFactory _httpRequestMessageFactory;


    public RestResourceCommandFactory(
        IHttpClientProvider httpClientProvider,
        IHttpRequestMessageFactory httpRequestMessageFactory,
        IHttpResponseHandlerFactory httpResponseHandlerFactory)
    {
        Ensure.NotNull(httpClientProvider, nameof(httpClientProvider));
        Ensure.NotNull(httpRequestMessageFactory, nameof(httpRequestMessageFactory));
        Ensure.NotNull(httpResponseHandlerFactory, nameof(httpResponseHandlerFactory));

        _httpClientProvider = httpClientProvider;
        _httpRequestMessageFactory = httpRequestMessageFactory;
        _httpResponseHandlerFactory = httpResponseHandlerFactory;
    }


    public IRestResourceCommand<TResponse> CreateGet<TResponse>(Uri resourceUri, params string[] additionalAcceptContentTypes)
        => new GetRestResourceCommand<TResponse>(
            _httpClientProvider,
            _httpRequestMessageFactory,
            _httpResponseHandlerFactory,
            resourceUri,
            additionalAcceptContentTypes);

    public IRestResourceCommand<string> CreatePlainText(Uri resourceUri, params string[] additionalAcceptContentTypes)
        => new GetWithPlainTextResultRestResourceCommand(
            _httpClientProvider,
            _httpRequestMessageFactory,
            _httpResponseHandlerFactory,
            resourceUri,
            additionalAcceptContentTypes);

    public IRestResourceCommand<TResponse> CreatePatch<TResponse>(Uri resourceUri)
        => new PatchRestResourceCommand<TResponse>(
            _httpClientProvider,
            _httpRequestMessageFactory,
            _httpResponseHandlerFactory,
            resourceUri);

    public IRestResourceCommand<TResponse> CreateDelete<TResponse>(Uri resourceUri)
        => new DeleteRestResourceCommand<TResponse>(
            _httpClientProvider,
            _httpRequestMessageFactory,
            _httpResponseHandlerFactory,
            resourceUri);

    public IRestResourceCommand<HttpStatusCode> CreateDeleteWithStatusCodeResult<TResponse>(Uri resourceUri)
        => new DeleteWithStatusCodeResultRestResourceCommand<TResponse>(
            _httpClientProvider,
            _httpRequestMessageFactory,
            _httpResponseHandlerFactory,
            resourceUri);

    public IRestResourceCommand<HttpStatusCode> CreatePostWithStatusCodeResult<TRequest>(Uri resourceUri, TRequest request)
        where TRequest : class
        => new PostWithStatusCodeResultRestResourceCommand<TRequest>(
            _httpClientProvider,
            _httpRequestMessageFactory,
            _httpResponseHandlerFactory,
            resourceUri,
            request);

    public IRestResourceCommand<TResponse> CreatePost<TRequest, TResponse>(Uri resourceUri, TRequest request)
        where TRequest : class
        => new PostRestResourceCommand<TRequest, TResponse>(
            _httpClientProvider,
            _httpRequestMessageFactory,
            _httpResponseHandlerFactory,
            resourceUri,
            request);

    public IRestResourceCommand<TResponse> CreatePost<TResponse>(Uri resourceUri)
        => new PostWithoutBodyRestResourceCommand<TResponse>(
            _httpClientProvider,
            _httpRequestMessageFactory,
            _httpResponseHandlerFactory,
            resourceUri);

    public IRestResourceCommand<TResponse> CreatePut<TRequest, TResponse>(Uri resourceUri, TRequest request)
        where TRequest : class
        => new PutRestResourceCommand<TRequest, TResponse>(
            _httpClientProvider,
            _httpRequestMessageFactory,
            _httpResponseHandlerFactory,
            resourceUri,
            request);

    public IRestResourceCommand<TResponse> CreatePatchWithContent<TRequest, TResponse>(Uri resourceUri, TRequest request)
        where TRequest : class
        => new PatchWithContentRestResourceCommand<TRequest, TResponse>(
            _httpClientProvider,
            _httpRequestMessageFactory,
            _httpResponseHandlerFactory,
            resourceUri,
            request);
}
