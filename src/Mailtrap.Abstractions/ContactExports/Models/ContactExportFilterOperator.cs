namespace Mailtrap.ContactExports.Models;


/// <summary>
/// Represents operator of the contact export filter.
/// </summary>
public sealed record ContactExportFilterOperator : StringEnum<ContactExportFilterOperator>
{
    /// <summary>
    /// Gets the value representing "equal" filter operator.
    /// </summary>
    ///
    /// <value>
    /// Represents "equal" filter operator.
    /// </value>
    public static readonly ContactExportFilterOperator Equal = Define("equal");
}
