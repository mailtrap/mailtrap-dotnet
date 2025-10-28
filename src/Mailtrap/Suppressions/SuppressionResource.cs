namespace Mailtrap.Suppressions;


internal sealed class SuppressionResource : RestResource, ISuppressionResource
{
    public SuppressionResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<Suppression> Delete(CancellationToken cancellationToken = default)
        => await Delete<Suppression>(cancellationToken).ConfigureAwait(false);
}
