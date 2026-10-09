namespace Mailtrap.Inbound;


internal sealed class InboundInboxContentResource : RestResource, IInboundInboxContentResource
{
    public InboundInboxContentResource(IRestResourceCommandFactory restResourceCommandFactory, Uri resourceUri)
        : base(restResourceCommandFactory, resourceUri) { }


    public IInboundMessageCollectionResource Messages()
        => new InboundMessageCollectionResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.MessagesSegment));

    public IInboundMessageResource Message(string messageId)
    {
        Ensure.NotNullOrEmpty(messageId, nameof(messageId));
        var encoded = Uri.EscapeDataString(messageId);

        return new InboundMessageResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.MessagesSegment).Append(encoded));
    }

    public IInboundThreadCollectionResource Threads()
        => new InboundThreadCollectionResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.ThreadsSegment));

    public IInboundThreadResource Thread(string threadId)
    {
        Ensure.NotNullOrEmpty(threadId, nameof(threadId));
        var encoded = Uri.EscapeDataString(threadId);

        return new InboundThreadResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.ThreadsSegment).Append(encoded));
    }

    public IInboundForwardRuleCollectionResource ForwardRules()
        => new InboundForwardRuleCollectionResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.ForwardRulesSegment));

    public IInboundForwardRuleResource ForwardRule(long forwardRuleId)
    {
        Ensure.GreaterThanZero(forwardRuleId, nameof(forwardRuleId));

        return new InboundForwardRuleResource(RestResourceCommandFactory, ResourceUri.Append(UrlSegments.ForwardRulesSegment).Append(forwardRuleId));
    }
}
