namespace Mailtrap.Inbound;


/// <summary>
/// Represents the token-scoped root of the Inbound Email API.
/// </summary>
public interface IInboundResource : IRestResource
{
    /// <summary>
    /// Gets the inbound folder collection resource.
    /// </summary>
    ///
    /// <returns>
    /// Inbound folder collection resource.
    /// </returns>
    public IInboundFolderCollectionResource Folders();

    /// <summary>
    /// Gets the resource for a specific inbound folder, identified by <paramref name="folderId"/>.
    /// </summary>
    ///
    /// <param name="folderId">
    /// ID of the folder to get resource for.
    /// </param>
    ///
    /// <returns>
    /// Resource for the folder with the specified ID.
    /// </returns>
    public IInboundFolderResource Folder(long folderId);

    /// <summary>
    /// Gets the message, thread, and forward rule resources for a specific inbox, identified by <paramref name="inboxId"/>.
    /// </summary>
    ///
    /// <param name="inboxId">
    /// ID of the inbox to get resource for.
    /// </param>
    ///
    /// <returns>
    /// Resource exposing the messages, threads, and forward rules of the inbox with the specified ID.
    /// </returns>
    ///
    /// <remarks>
    /// Inbox management (create/list/get/update/delete) is folder-scoped - see
    /// <see cref="IInboundFolderResource.Inboxes"/> and <see cref="IInboundFolderResource.Inbox(long)"/>.
    /// </remarks>
    public IInboundInboxContentResource Inbox(long inboxId);
}
