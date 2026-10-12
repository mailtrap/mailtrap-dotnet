namespace Mailtrap.Inbound;


/// <summary>
/// Represents access to the messages, threads, and forward rules of an inbound inbox.
/// </summary>
public interface IInboundInboxContentResource : IRestResource
{
    /// <summary>
    /// Gets the message collection resource for this inbox.
    /// </summary>
    ///
    /// <returns>
    /// Message collection resource for this inbox.
    /// </returns>
    public IInboundMessageCollectionResource Messages();

    /// <summary>
    /// Gets the resource for a specific message in this inbox, identified by <paramref name="messageId"/>.
    /// </summary>
    ///
    /// <param name="messageId">
    /// ID of the message to get resource for.
    /// </param>
    ///
    /// <returns>
    /// Resource for the message with the specified ID.
    /// </returns>
    public IInboundMessageResource Message(string messageId);

    /// <summary>
    /// Gets the thread collection resource for this inbox.
    /// </summary>
    ///
    /// <returns>
    /// Thread collection resource for this inbox.
    /// </returns>
    public IInboundThreadCollectionResource Threads();

    /// <summary>
    /// Gets the resource for a specific thread in this inbox, identified by <paramref name="threadId"/>.
    /// </summary>
    ///
    /// <param name="threadId">
    /// ID of the thread to get resource for.
    /// </param>
    ///
    /// <returns>
    /// Resource for the thread with the specified ID.
    /// </returns>
    public IInboundThreadResource Thread(string threadId);

    /// <summary>
    /// Gets the forward rule collection resource for this inbox.
    /// </summary>
    ///
    /// <returns>
    /// Forward rule collection resource for this inbox.
    /// </returns>
    public IInboundForwardRuleCollectionResource ForwardRules();

    /// <summary>
    /// Gets the resource for a specific forward rule of this inbox, identified by <paramref name="forwardRuleId"/>.
    /// </summary>
    ///
    /// <param name="forwardRuleId">
    /// ID of the forward rule to get resource for.
    /// </param>
    ///
    /// <returns>
    /// Resource for the forward rule with the specified ID.
    /// </returns>
    public IInboundForwardRuleResource ForwardRule(long forwardRuleId);
}
