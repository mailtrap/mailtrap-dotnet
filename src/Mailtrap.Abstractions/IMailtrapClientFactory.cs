namespace Mailtrap;


/// <summary>
/// Factory to spawn <see cref="IMailtrapClient"/> implementation instances.
/// </summary>
///
/// <remarks>
/// Primary use case is for scenarios when usage of DI container is not possible or desired. <br />
/// Disposable.
/// </remarks>
public interface IMailtrapClientFactory : IDisposable
{
    /// <summary>
    /// Creates new instance of <see cref="IMailtrapClient"/>.
    /// </summary>
    /// 
    /// <returns>
    /// New <see cref="IMailtrapClient"/> instance.
    /// </returns>
    ///
    /// <exception cref="ObjectDisposedException">
    /// When factory was disposed.
    /// </exception>
    ///
    /// <remarks>
    /// Each call to this method is guaranteed to return a new instance of <see cref="IMailtrapClient"/>.
    /// </remarks>
    public IMailtrapClient CreateClient();

    /// <summary>
    /// Creates new instance of <see cref="IMailtrapOrganizationClient"/> for organization-scoped operations.
    /// </summary>
    ///
    /// <returns>
    /// New <see cref="IMailtrapOrganizationClient"/> instance.
    /// </returns>
    ///
    /// <exception cref="ObjectDisposedException">
    /// When factory was disposed.
    /// </exception>
    public IMailtrapOrganizationClient CreateOrganizationClient();
}
