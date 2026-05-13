namespace Mailtrap.Core.Rest;


internal interface IRestResourceCommandFactory
{
    public IRestResourceCommand<TResponse> CreateGet<TResponse>(Uri resourceUri, params string[] additionalAcceptContentTypes);
    public IRestResourceCommand<string> CreatePlainText(Uri resourceUri, params string[] additionalAcceptContentTypes);
    public IRestResourceCommand<TResponse> CreatePatch<TResponse>(Uri resourceUri);
    public IRestResourceCommand<TResponse> CreateDelete<TResponse>(Uri resourceUri);
    public IRestResourceCommand<HttpStatusCode> CreateDeleteWithStatusCodeResult<TResponse>(Uri resourceUri);
    public IRestResourceCommand<HttpStatusCode> CreatePostWithStatusCodeResult<TRequest>(Uri resourceUri, TRequest request) where TRequest : class;
    public IRestResourceCommand<TResponse> CreatePost<TRequest, TResponse>(Uri resourceUri, TRequest request) where TRequest : class;
    public IRestResourceCommand<TResponse> CreatePost<TResponse>(Uri resourceUri);
    public IRestResourceCommand<TResponse> CreatePut<TRequest, TResponse>(Uri resourceUri, TRequest request) where TRequest : class;
    public IRestResourceCommand<TResponse> CreatePatchWithContent<TRequest, TResponse>(Uri resourceUri, TRequest request) where TRequest : class;
}
