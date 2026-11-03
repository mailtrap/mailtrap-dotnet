using Mailtrap.TestingMessages.Responses;


namespace Mailtrap.Example.ApiUsage.Logic;


internal sealed class MessageReactor : Reactor
{
    private readonly TestSendReactor _testSendReactor;


    public MessageReactor(
        TestSendReactor testSendReactor,
        IMailtrapClient mailtrapClient,
        ILogger<MessageReactor> logger)
        : base(mailtrapClient, logger)
    {
        _testSendReactor = testSendReactor;
    }


    public async Task Process(long accountId, long sandboxId)
    {
        await _testSendReactor.Send(sandboxId);

        IInboxResource inboxResource = _mailtrapClient
            .Account(accountId)
            .Sandbox(sandboxId);

        // Get resource for message collection
        ITestingMessageCollectionResource messagesResource = inboxResource.Messages();

        // Fetch messages from the sandbox
        var messageFilter = new TestingMessageFilter
        {
            SearchFilter = "Supervision request"
        };
        IList<TestingMessage> messages = await messagesResource.Fetch(messageFilter);

        ITestingMessageResource messageResource;

        TestingMessage? message = messages.FirstOrDefault();
        if (message is not null)
        {
            // Get resource for message
            messageResource = inboxResource.Message(message.Id);

            // Get message details
            message = await messageResource.GetDetails();
            _logger.LogInformation("Message: {Message}", message);
        }

        // Fetch messages from the sandbox
        messageFilter = new TestingMessageFilter
        {
            SearchFilter = "Greetings"
        };
        messages = await messagesResource.Fetch(messageFilter);

        if (messages.Count == 0)
        {
            _logger.LogWarning("No greetings messages found.");
        }
        else
        {
            _logger.LogInformation("Found {Count} greetings messages.", messages.Count);
        }

        // Another fetch
        messageFilter = new TestingMessageFilter
        {
            SearchFilter = "hero.bill"
        };
        messages = await messagesResource.Fetch(messageFilter);

        message = messages.FirstOrDefault();

        if (message is null)
        {
            _logger.LogWarning("No messages found.");
            return;
        }

        // Get resource for message
        messageResource = inboxResource.Message(message.Id);

        // Get message details
        message = await messageResource.GetDetails();
        _logger.LogInformation("Message: {Message}", message);

        // Get message headers
        TestingMessageHeaders headers = await messageResource.GetHeaders();
        _logger.LogInformation("Message headers: {Headers}", headers.Headers);

        // Get raw message content
        string rawMessageContent = await messageResource.AsRaw();
        _logger.LogInformation("Raw message:\n{Message}", rawMessageContent);

        // Get EML message content
        string emlMessageContent = await messageResource.AsEml();
        _logger.LogInformation("EML message:\n{Message}", emlMessageContent);

        if (message.TextBodySize > 0)
        {
            // Get plain text message body
            string textMessageBody = await messageResource.GetTextBody();
            _logger.LogInformation("Plain text message body:\n{Message}", textMessageBody);
        }

        if (message.HtmlBodySize > 0)
        {
            // Get HTML message body
            string htmlMessageBody = await messageResource.GetHtmlBody();
            _logger.LogInformation("HTML message body:\n{Message}", htmlMessageBody);

            // Get HTML message source
            string htmlMessageSource = await messageResource.GetHtmlSource();
            _logger.LogInformation("HTML message source:\n{Message}", htmlMessageSource);

            // Get HTML analysis report for message 
            TestingMessageHtmlReport htmlReport = await messageResource.GetHtmlAnalysisReport();
            _logger.LogInformation("HTML analysis report: {Report}", htmlReport.Report);
        }

        // Get spam report for message 
        TestingMessageSpamReport spamReport = await messageResource.GetSpamReport();
        _logger.LogInformation("Spam report: {Report}", spamReport.Report);

        // Update message details
        var updateMessageRequest = new UpdateTestingMessageRequest(true);
        TestingMessage updatedMessage = await messageResource.Update(updateMessageRequest);
        _logger.LogInformation("Updated Message: {Message}", updatedMessage);

        // Forward message
        var forwardMessageRequest = new ForwardTestingMessageRequest("forward@domain.com");
        ForwardTestingMessageResponse forwardedMessage = await messageResource.Forward(forwardMessageRequest);
        _logger.LogInformation("Message forwarded: {Forward}", forwardedMessage.Message);

        // Delete message
        // Beware that resource becomes invalid after deletion and should not be used anymore
        TestingMessage deletedMessage = await messageResource.Delete();
        _logger.LogInformation("Deleted Message: {Message}", deletedMessage);
    }
}
