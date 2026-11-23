namespace Mailtrap.Contacts;

/// <summary>
/// Represents Contact resource.
/// </summary>
public interface IContactResource : IRestResource
{
    /// <summary>
    /// Gets details of the contact, represented by the current resource instance.
    /// </summary>
    ///
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    ///
    /// <returns>
    /// Requested contact details.
    /// </returns>
    public Task<ContactResponse> GetDetails(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the contact, represented by the current resource instance, with details specified by <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// Contact details for update.
    /// </param>
    ///
    /// <param name="cancellationToken">
    /// <inheritdoc cref="GetDetails(CancellationToken)" path="/param[@name='cancellationToken']"/>
    /// </param>
    ///
    /// <returns>
    /// Updated contact details.
    /// </returns>
    public Task<UpdateContactResponse> Update(UpdateContactRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a contact, represented by the current resource instance.
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
    /// After deletion of the contact, represented by the current resource instance, it will be no longer available.<br />
    /// Thus any further operations on it will result in an error.
    /// </para>
    /// </remarks>
    public Task Delete(CancellationToken cancellationToken = default);
}
