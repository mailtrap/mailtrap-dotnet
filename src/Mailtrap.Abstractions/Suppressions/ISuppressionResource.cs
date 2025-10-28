namespace Mailtrap.Suppressions;


/// <summary>
/// Represents suppression resource.
/// </summary>
public interface ISuppressionResource : IRestResource
{
    /// <summary>
    /// Deletes a suppression, represented by the current resource instance.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token to control operation cancellation.
    /// </param>
    /// <returns>
    /// Deleted suppression details.
    /// </returns>
    /// <remarks>
    /// After deletion, the system will no longer prevent sending to this email unless it's recorded in suppressions again.
    /// </remarks>
    public Task<Suppression> Delete(CancellationToken cancellationToken = default);
}
