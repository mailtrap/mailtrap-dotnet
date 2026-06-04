namespace Mailtrap.EmailCampaigns;


/// <summary>
/// Represents email campaign collection resource.
/// </summary>
public interface IEmailCampaignCollectionResource : IRestResource
{
    /// <summary>
    /// Gets a paginated list of email campaigns for the account, newest first.
    /// </summary>
    ///
    /// <param name="filter">
    /// Optional filtering and pagination parameters.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// A page of email campaigns and the pagination metadata.
    /// </returns>
    public Task<EmailCampaignList> GetAll(EmailCampaignListFilter? filter = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new email campaign with details specified by <paramref name="request"/>.<br/>
    /// The created campaign is returned in the <see cref="CampaignState.Draft"/> state.
    /// </summary>
    ///
    /// <param name="request">
    /// Request containing email campaign details for creation.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// <inheritdoc cref="GetAll(EmailCampaignListFilter, CancellationToken)" path="/param[@name='cancellationToken']"/>
    /// </param>
    ///
    /// <returns>
    /// Created email campaign details.
    /// </returns>
    public Task<EmailCampaign> Create(CreateEmailCampaignRequest request, CancellationToken cancellationToken = default);
}
