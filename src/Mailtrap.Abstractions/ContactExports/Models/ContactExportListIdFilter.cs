namespace Mailtrap.ContactExports.Models;

/// <summary>
/// Represents a filter by list ids for contact export operations.
/// </summary>
public sealed record ContactExportListIdFilter : ContactExportFilterBase
{
    /// <summary>
    /// Discriminator value for JSON polymorphic deserialization.
    /// </summary>
    [JsonIgnore]
    public const string Discriminator = "list_id";

    /// <summary>
    /// Gets contact export filter name.
    /// </summary>
    /// <value>
    /// Contact export filter name.
    /// </value>
    [JsonIgnore] // Marked as ignored to avoid serialization, as it's handled by the TypeDiscriminator
    public override ContactExportFilterName Name => ContactExportFilterName.ListId;

    /// <summary>
    /// Gets contact export filter IDs list.
    /// </summary>
    ///
    /// <value>
    /// Contact export filter IDs list.
    /// </value>
    [JsonPropertyName("value")]
    [JsonPropertyOrder(3)]
    public IList<int> Value { get; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactExportListIdFilter"/> class with provided values.
    /// </summary>
    /// <param name="value">
    /// Collection of list IDs.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is <c>null</c> or empty.
    /// </exception>
    /// <remarks>
    /// This constructor is required for JSON deserialization.
    /// Operator is set to Equal by default.
    /// </remarks>
    [JsonConstructor]
    public ContactExportListIdFilter(IList<int> value) : this((IEnumerable<int>)value)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactExportListIdFilter"/> class with provided values.
    /// </summary>
    /// <param name="value">
    /// Collection of list IDs.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is <c>null</c> or empty.
    /// </exception>
    /// <remarks>
    /// Operator is set to Equal by default.
    /// </remarks>
    public ContactExportListIdFilter(IEnumerable<int> value)
    {
        Ensure.NotNullOrEmpty(value, nameof(value));

        Value = value.Clone(); // defensive copy to prevent post-ctor mutation
        Operator = ContactExportFilterOperator.Equal;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactExportListIdFilter"/> class with provided values.
    /// </summary>
    /// <param name="values">
    /// Collection of list IDs.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="values"/> is <c>null</c> or empty.
    /// </exception>
    /// <remarks>
    /// Operator is set to Equal by default.
    /// </remarks>
    public ContactExportListIdFilter(params int[] values)
    {
        Ensure.NotNullOrEmpty(values, nameof(values));

        Value = new List<int>(values);
        Operator = ContactExportFilterOperator.Equal;
    }
}
