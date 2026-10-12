using Mailtrap.Emails.Models;
using Mailtrap.Inbound;
using Mailtrap.Inbound.Models;
using Mailtrap.Inbound.Requests;
using Mailtrap.Inbound.Responses;
using Microsoft.Extensions.Logging;


namespace Mailtrap.Example.Inbound;


internal sealed class InboundReactor
{
    private readonly IMailtrapClient _client;
    private readonly ILogger<InboundReactor> _logger;


    public InboundReactor(IMailtrapClient client, ILogger<InboundReactor> logger)
    {
        _client = client;
        _logger = logger;
    }


    public async Task Process()
    {
        IInboundResource inbound = _client.Inbound();

        InboundFolder folder = await ProcessFolders(inbound);
        InboundInbox inbox = await ProcessInboxes(inbound.Folder(folder.Id));

        await ProcessForwardRules(inbound.Inbox(inbox.Id));
        await ProcessMessages(inbound.Inbox(inbox.Id));
        await ProcessThreads(inbound.Inbox(inbox.Id));

        // Cleanup.
        await inbound.Folder(folder.Id).Inbox(inbox.Id).Delete();
        await inbound.Folder(folder.Id).Delete();
    }


    private async Task<InboundFolder> ProcessFolders(IInboundResource inbound)
    {
        IList<InboundFolder> folders = await inbound.Folders().GetAll();
        _logger.LogInformation("Found {Count} inbound folder(s).", folders.Count);

        InboundFolder folder = await inbound.Folders().Create(new CreateInboundFolderRequest { Name = "Support" });
        _logger.LogInformation("Created folder: Id={Id}, Name={Name}", folder.Id, folder.Name);

        IInboundFolderResource folderResource = inbound.Folder(folder.Id);

        InboundFolder details = await folderResource.GetDetails();
        _logger.LogInformation("Folder details: Name={Name}", details.Name);

        await folderResource.Update(new UpdateInboundFolderRequest { Name = "Support (renamed)" });

        return folder;
    }

    private async Task<InboundInbox> ProcessInboxes(IInboundFolderResource folderResource)
    {
        // Omit DomainId for a Mailtrap-hosted inbox; set it to create a custom-domain (catch-all) inbox.
        InboundInbox inbox = await folderResource.Inboxes().Create(new CreateInboundInboxRequest { Name = "Support inbox" });
        _logger.LogInformation("Created inbox: Id={Id}, Address={Address}", inbox.Id, inbox.Address);

        IList<InboundInbox> inboxes = await folderResource.Inboxes().GetAll();
        _logger.LogInformation("Folder has {Count} inbox(es).", inboxes.Count);

        await folderResource.Inbox(inbox.Id).Update(new UpdateInboundInboxRequest { Name = "Support inbox (renamed)" });

        return inbox;
    }

    private async Task ProcessForwardRules(IInboundInboxContentResource inboxContent)
    {
        InboundForwardRule rule = await inboxContent.ForwardRules().Create(new CreateInboundForwardRuleRequest
        {
            Name = "Copy billing mail to finance",
            Conditions =
            [
                new InboundForwardRuleCondition
                {
                    MatchType = ForwardRuleMatchType.Sender,
                    Operator = ForwardRuleOperator.EndsWith,
                    Value = "@billing.example.com"
                }
            ],
            Destinations = [new InboundForwardRuleDestination { Email = "finance@example.com" }]
        });
        _logger.LogInformation("Created forward rule: Id={Id}, Name={Name}", rule.Id, rule.Name);

        IList<InboundForwardRule> rules = await inboxContent.ForwardRules().GetAll();
        _logger.LogInformation("Inbox has {Count} forward rule(s).", rules.Count);

        IInboundForwardRuleResource ruleResource = inboxContent.ForwardRule(rule.Id);

        InboundForwardRule details = await ruleResource.GetDetails();
        _logger.LogInformation("Forward rule details: Name={Name}", details.Name);

        await ruleResource.Update(new UpdateInboundForwardRuleRequest { Name = "Copy billing mail to finance (renamed)" });

        await ruleResource.Delete();
    }

    private async Task ProcessMessages(IInboundInboxContentResource inboxContent)
    {
        // List received messages. Pass the previous page's LastId to fetch the next page.
        InboundMessagesListResponse messages = await inboxContent.Messages().List();
        _logger.LogInformation("Inbox has {Total} message(s).", messages.TotalCount);

        if (messages.Data.Count == 0)
        {
            return;
        }

        IInboundMessageResource messageResource = inboxContent.Message(messages.Data[0].Id!);

        // Get a single message with its body and attachment download URLs.
        InboundMessage message = await messageResource.GetDetails();
        _logger.LogInformation("Message subject: {Subject}", message.Subject);

        foreach (ForwardOutcome forward in message.Forwards)
        {
            _logger.LogInformation("Forwarded by rule {RuleId} to {Destination}: {Status} {Reason}",
                forward.RuleId, forward.Destination, forward.Status, forward.Reason);
        }

        // Reply to a message (sends a real email to the original sender).
        SendMessageResult reply = await messageResource.Reply(new ReplyInboundMessageRequest
        {
            Text = "Thanks for reaching out!",
            Html = "<p>Thanks for reaching out!</p>"
        });
        _logger.LogInformation("Reply sent: {Ids}", string.Join(",", reply.MessageIds));

        // Reply to a message and copy the original's other recipients.
        await messageResource.ReplyAll(new ReplyInboundMessageRequest { Text = "Looping everyone in." });

        // Forward a message to new recipients (at least one To is required).
        await messageResource.Forward(new ForwardInboundMessageRequest
        {
            To = [new EmailAddress("colleague@example.com")],
            Text = "Please take a look."
        });

        // Delete a message.
        await messageResource.Delete();
    }

    private async Task ProcessThreads(IInboundInboxContentResource inboxContent)
    {
        InboundThreadsListResponse threads = await inboxContent.Threads().List();
        _logger.LogInformation("Inbox has {Total} thread(s).", threads.TotalCount);

        InboundThreadsListResponse found = await inboxContent.Threads().List(lastId: null, search: "acme");
        _logger.LogInformation("Found {Total} thread(s) matching 'acme'.", found.TotalCount);

        if (threads.Data.Count == 0)
        {
            return;
        }

        IInboundThreadResource threadResource = inboxContent.Thread(threads.Data[0].Id!);

        // Get a single thread with its messages embedded (oldest first).
        InboundThread thread = await threadResource.GetDetails();
        _logger.LogInformation("Thread has {Count} message(s).", thread.Messages.Count);

        foreach (InboundThreadMessage threadMessage in thread.Messages)
        {
            if (threadMessage.Delivery is not null)
            {
                _logger.LogInformation("Sent to {To}: {Status}", threadMessage.Delivery.To, threadMessage.Delivery.Status);
            }

            _logger.LogInformation("Message {Id} was forwarded to {Count} destination(s).",
                threadMessage.Id, threadMessage.Forwards.Count);
        }

        // Delete a thread (inbound messages are removed; sent messages are preserved).
        await threadResource.Delete();
    }
}
