namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Event type values for the events filter (API: delivery, open, click, bounce, spam, unsubscribe, soft_bounce, reject, suspension).
/// </summary>
public sealed record EmailLogEventType : StringEnum<EmailLogEventType>
{
    /// <summary>Delivery event.</summary>
    public static readonly EmailLogEventType Delivery = Define("delivery");
    /// <summary>Open event.</summary>
    public static readonly EmailLogEventType Open = Define("open");
    /// <summary>Click event.</summary>
    public static readonly EmailLogEventType Click = Define("click");
    /// <summary>Bounce event.</summary>
    public static readonly EmailLogEventType Bounce = Define("bounce");
    /// <summary>Spam event.</summary>
    public static readonly EmailLogEventType Spam = Define("spam");
    /// <summary>Unsubscribe event.</summary>
    public static readonly EmailLogEventType Unsubscribe = Define("unsubscribe");
    /// <summary>Soft bounce event.</summary>
    public static readonly EmailLogEventType SoftBounce = Define("soft_bounce");
    /// <summary>Reject event.</summary>
    public static readonly EmailLogEventType Reject = Define("reject");
    /// <summary>Suspension event.</summary>
    public static readonly EmailLogEventType Suspension = Define("suspension");
}
