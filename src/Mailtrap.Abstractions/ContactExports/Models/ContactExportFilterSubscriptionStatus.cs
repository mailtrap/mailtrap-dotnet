namespace Mailtrap.ContactExports.Models;


/// <summary>
/// Represents subscription status of the contact export filter.
/// </summary>
public sealed record ContactExportFilterSubscriptionStatus : StringEnum<ContactExportFilterSubscriptionStatus>
{
    /// <summary>
    /// Gets the value representing "subscribed" filter subscription status.
    /// </summary>
    ///
    /// <value>
    /// Represents "subscribed" filter subscription status.
    /// </value>
    public static readonly ContactExportFilterSubscriptionStatus Subscribed = Define("subscribed");

    /// <summary>
    /// Gets the value representing "unsubscribed" filter subscription status.
    /// </summary>
    ///
    /// <value>
    /// Represents "unsubscribed" filter subscription status.
    /// </value>
    public static readonly ContactExportFilterSubscriptionStatus Unsubscribed = Define("unsubscribed");
}
