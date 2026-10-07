namespace Mailtrap.Templates;


/// <summary>
/// Represents Template resource, served by the <c>/api/templates</c> endpoints.
/// </summary>
/// <remarks>
/// The endpoints are experimental: their request and response shapes may change before general availability.
/// </remarks>
public interface ITemplateResource : IRestResource
{
    /// <summary>
    /// Gets details of the template, represented by the current resource instance.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Requested template details.
    /// </returns>
    public Task<Template> GetDetails(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the template, represented by the current resource instance, with details specified by <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// Template details for update.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// <inheritdoc cref="GetDetails(CancellationToken)" path="/param[@name='cancellationToken']"/>
    /// </param>
    ///
    /// <returns>
    /// Updated template details.
    /// </returns>
    public Task<Template> Update(UpdateTemplateRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the template, represented by the current resource instance.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// <inheritdoc cref="GetDetails(CancellationToken)" path="/param[@name='cancellationToken']"/>
    /// </param>
    ///
    /// <returns>
    /// Nothing is returned upon successful deletion.
    /// </returns>
    ///
    /// <remarks>
    /// <para>
    /// After deletion of the template, represented by the current resource instance, it will no longer be available.<br />
    /// Thus any further operations on it will result in an error.
    /// </para>
    /// </remarks>
    public Task Delete(CancellationToken cancellationToken = default);
}
