namespace Mailtrap.EmailCampaigns;


/// <summary>
/// Represents a single email campaign resource.
/// </summary>
public interface IEmailCampaignResource : IRestResource
{
    /// <summary>
    /// Gets the details of the email campaign, represented by this resource instance.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Email campaign details.
    /// </returns>
    public Task<EmailCampaign> GetDetails(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the email campaign, represented by this resource instance, with details specified
    /// by <paramref name="request"/>.<br/>
    /// The campaign must not be in a sending state. Only the provided attributes are changed.
    /// </summary>
    ///
    /// <param name="request">
    /// Request containing email campaign details to update.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// <inheritdoc cref="GetDetails(CancellationToken)" path="/param[@name='cancellationToken']"/>
    /// </param>
    ///
    /// <returns>
    /// Updated email campaign details.
    /// </returns>
    public Task<EmailCampaign> Update(UpdateEmailCampaignRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the email campaign, represented by this resource instance.<br/>
    /// The campaign must not be in a sending state.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// <inheritdoc cref="GetDetails(CancellationToken)" path="/param[@name='cancellationToken']"/>
    /// </param>
    ///
    /// <returns>
    /// The deleted email campaign.
    /// </returns>
    ///
    /// <remarks>
    /// On success the API returns HTTP 200 with the deleted campaign object in the body.
    /// After deletion, the resource represented by this instance should no longer be used.
    /// </remarks>
    public Task<EmailCampaign> Delete(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets aggregated performance statistics for the email campaign, represented by this resource instance.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// <inheritdoc cref="GetDetails(CancellationToken)" path="/param[@name='cancellationToken']"/>
    /// </param>
    ///
    /// <returns>
    /// Aggregated campaign statistics. All counts and rates are <c>0</c> when the campaign has not been started.
    /// </returns>
    public Task<EmailCampaignStats> GetStats(CancellationToken cancellationToken = default);
}
