namespace Mailtrap.ContactExports.Models;


/// <summary>
/// Represents name of the contact export filter.
/// </summary>
public sealed record ContactExportFilterName : StringEnum<ContactExportFilterName>
{
    /// <summary>
    /// Gets the value representing "list_id" filter name.
    /// </summary>
    ///
    /// <value>
    /// Represents "list_id" filter name.
    /// </value>
    public static readonly ContactExportFilterName ListId = Define(ContactExportListIdFilter.Discriminator);

    /// <summary>
    /// Gets the value representing "subscription_status" filter name.
    /// </summary>
    ///
    /// <value>
    /// Represents "subscription_status" filter name.
    /// </value>
    public static readonly ContactExportFilterName SubscriptionStatus = Define(ContactExportSubscriptionStatusFilter.Discriminator);
}
