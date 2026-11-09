namespace Mailtrap.Example.ApiUsage.Logic;


internal sealed class SandboxReactor : Reactor
{
    private readonly SandboxMessageReactor _sandboxMessageReactor;


    public SandboxReactor(
        SandboxMessageReactor sandboxMessageReactor,
        IMailtrapClient mailtrapClient,
        ILogger<SandboxReactor> logger)
        : base(mailtrapClient, logger)
    {
        _sandboxMessageReactor = sandboxMessageReactor;
    }


    public async Task Process(long accountId, long projectId)
    {
        IAccountResource accountResource = _mailtrapClient.Account(accountId);

        var inboxName = "My Test Inbox";

        // Get resource for sandbox collection
        IInboxCollectionResource inboxesResource = accountResource.Sandboxes();

        // Get all sandboxes for account
        IList<Inbox> inboxes = await inboxesResource.GetAll();

        Inbox? inbox = inboxes
            .FirstOrDefault(i => string.Equals(i.Name, inboxName, StringComparison.OrdinalIgnoreCase));

        if (inbox is null)
        {
            _logger.LogWarning("No sandbox found. Creating.");

            // Create sandbox
            var createInboxRequest = new CreateInboxRequest(projectId, inboxName);
            inbox = await inboxesResource.Create(createInboxRequest);
        }
        else
        {
            _logger.LogInformation("Sandbox found.");
        }

        // Get resource for specific sandbox
        IInboxResource inboxResource = accountResource.Sandbox(inbox.Id);

        // Get Details
        inbox = await inboxResource.GetDetails();
        _logger.LogInformation("Sandbox: {Sandbox}", inbox);

        // Process messages
        await _sandboxMessageReactor.Process(accountId, inbox.Id);

        // Mark all messages in the sandbox as read
        Inbox updatedInbox = await inboxResource.MarkAsRead();

        // Update sandbox details
        var updateInboxRequest = new UpdateInboxRequest
        {
            Name = "Updated Inbox Name"
        };
        updatedInbox = await inboxResource.Update(updateInboxRequest);

        // Toggle email for sandbox
        updatedInbox = await inboxResource.ToggleEmailAddress();

        // Reset email address for sandbox
        updatedInbox = await inboxResource.ResetEmailAddress();

        // Reset credentials for sandbox
        updatedInbox = await inboxResource.ResetCredentials();

        _logger.LogInformation("Updated Sandbox: {Sandbox}", updatedInbox);

        // Delete sandbox
        // Beware that resource becomes invalid after deletion and should not be used anymore
        Inbox deletedInbox = await inboxResource.Delete();
        _logger.LogInformation("Deleted Sandbox: {Sandbox}", deletedInbox);
    }
}
