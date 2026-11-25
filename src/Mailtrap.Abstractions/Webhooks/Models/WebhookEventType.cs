namespace Mailtrap.Webhooks.Models;


/// <summary>
/// Event type the <c>email_sending</c> and <c>campaigns</c> webhooks can subscribe to.
/// </summary>
public sealed record WebhookEventType : StringEnum<WebhookEventType>
{
    /// <summary>
    /// Gets the value representing "delivery" event.
    /// </summary>
    public static readonly WebhookEventType Delivery = Define("delivery");

    /// <summary>
    /// Gets the value representing "soft_bounce" event.
    /// </summary>
    public static readonly WebhookEventType SoftBounce = Define("soft_bounce");

    /// <summary>
    /// Gets the value representing "bounce" event.
    /// </summary>
    public static readonly WebhookEventType Bounce = Define("bounce");

    /// <summary>
    /// Gets the value representing "suspension" event.
    /// </summary>
    public static readonly WebhookEventType Suspension = Define("suspension");

    /// <summary>
    /// Gets the value representing "unsubscribe" event.
    /// </summary>
    public static readonly WebhookEventType Unsubscribe = Define("unsubscribe");

    /// <summary>
    /// Gets the value representing "open" event.
    /// </summary>
    public static readonly WebhookEventType Open = Define("open");

    /// <summary>
    /// Gets the value representing "spam_complaint" event.
    /// </summary>
    public static readonly WebhookEventType SpamComplaint = Define("spam_complaint");

    /// <summary>
    /// Gets the value representing "click" event.
    /// </summary>
    public static readonly WebhookEventType Click = Define("click");

    /// <summary>
    /// Gets the value representing "reject" event.
    /// </summary>
    public static readonly WebhookEventType Reject = Define("reject");

    /// <summary>
    /// Gets the value representing "subscription_preferences_update" event:
    /// a recipient changed their subscription preferences.
    /// </summary>
    ///
    /// <remarks>
    /// Available only for <c>campaigns</c> webhooks and <c>email_sending</c> webhooks of the bulk stream;
    /// the API rejects it for the transactional stream.
    /// </remarks>
    public static readonly WebhookEventType SubscriptionPreferencesUpdate = Define("subscription_preferences_update");
}
