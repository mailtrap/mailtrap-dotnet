namespace Mailtrap.ContactExports.Models;

/// <summary>
/// Represents a filter by subscription status for contact export operations.
/// </summary>
public sealed record ContactExportSubscriptionStatusFilter : ContactExportFilterBase
{
    /// <summary>
    /// Discriminator value for JSON polymorphic deserialization.
    /// </summary>
    public const string Discriminator = "subscription_status";

    /// <summary>
    /// Gets contact export filter name.
    /// </summary>
    /// <value>
    /// Contact export filter name.
    /// </value>
    [JsonIgnore] // Marked as ignored to avoid serialization, as it's handled by the TypeDiscriminator
    public override ContactExportFilterName Name => ContactExportFilterName.SubscriptionStatus;

    /// <summary>
    /// Gets or sets contact export filter value.
    /// </summary>
    ///
    /// <value>
    /// Contact export filter value.
    /// </value>
    [JsonPropertyName("value")]
    [JsonPropertyOrder(3)]
    public ContactExportFilterSubscriptionStatus Value { get; set; } = ContactExportFilterSubscriptionStatus.Unknown;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactExportSubscriptionStatusFilter"/> class with specified value.
    /// </summary>
    /// <param name="value">
    /// Subscription status value.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// Operator is set to Equal by default.
    /// </remarks>
    [JsonConstructor]
    public ContactExportSubscriptionStatusFilter(ContactExportFilterSubscriptionStatus value)
    {
        Ensure.NotNull(value, nameof(value));

        Value = value;
        Operator = ContactExportFilterOperator.Equal;
    }
}
