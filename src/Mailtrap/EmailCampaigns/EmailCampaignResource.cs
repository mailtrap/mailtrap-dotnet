namespace Mailtrap.EmailCampaigns;


internal sealed class EmailCampaignResource : RestResource, IEmailCampaignResource
{
    public EmailCampaignResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public async Task<EmailCampaign> GetDetails(CancellationToken cancellationToken = default)
        => await Get<EmailCampaign>(cancellationToken).ConfigureAwait(false);

    public async Task<EmailCampaign> Update(UpdateEmailCampaignRequest request, CancellationToken cancellationToken = default)
    {
        Ensure.NotNull(request, nameof(request));

        return await Update<UpdateEmailCampaignRequestDto, EmailCampaign>(request.ToDto(), cancellationToken).ConfigureAwait(false);
    }

    public async Task<EmailCampaign> Delete(CancellationToken cancellationToken = default)
        => await Delete<EmailCampaign>(cancellationToken).ConfigureAwait(false);

    public async Task<EmailCampaignStats> GetStats(CancellationToken cancellationToken = default)
        => await Get<EmailCampaignStats>(UrlSegments.StatsSegment, cancellationToken).ConfigureAwait(false);
}
