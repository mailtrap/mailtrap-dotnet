namespace Mailtrap.Inbound.Models;


/// <summary>
/// The part of a message a forward rule condition is compared against.
/// </summary>
public sealed record ForwardRuleMatchType : StringEnum<ForwardRuleMatchType>
{
    /// <summary>
    /// Message sender.
    /// </summary>
    public static readonly ForwardRuleMatchType Sender = Define("sender");

    /// <summary>
    /// Message recipient.
    /// </summary>
    public static readonly ForwardRuleMatchType Recipient = Define("recipient");

    /// <summary>
    /// Message header.
    /// </summary>
    public static readonly ForwardRuleMatchType Header = Define("header");
}
