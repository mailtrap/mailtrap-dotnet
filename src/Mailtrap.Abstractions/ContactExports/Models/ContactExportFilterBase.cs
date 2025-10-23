namespace Mailtrap.ContactExports.Models;

/// <summary>
/// Generic filter object for contact export operations.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "name")]
[JsonDerivedType(typeof(ContactExportListIdFilter), ContactExportListIdFilter.Discriminator)]
[JsonDerivedType(typeof(ContactExportSubscriptionStatusFilter), ContactExportSubscriptionStatusFilter.Discriminator)]
public abstract record ContactExportFilterBase
{
    /// <summary>
    /// Gets contact export filter name.
    /// </summary>
    ///
    /// <value>
    /// Contact export filter name.
    /// </value>
    [JsonIgnore] // Marked as ignored to avoid serialization, as it's handled by the TypeDiscriminator
    public abstract ContactExportFilterName Name { get; }

    /// <summary>
    /// Gets or sets contact export filter operator.
    /// </summary>
    ///
    /// <value>
    /// Contact export filter operator.
    /// </value>
    [JsonPropertyName("operator")]
    [JsonPropertyOrder(2)]
    public ContactExportFilterOperator Operator { get; set; } = ContactExportFilterOperator.Unknown;
}
