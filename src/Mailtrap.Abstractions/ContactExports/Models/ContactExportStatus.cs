namespace Mailtrap.ContactExports.Models;


/// <summary>
/// Represents status of the contact export.
/// </summary>
public sealed record ContactExportStatus : StringEnum<ContactExportStatus>
{
    /// <summary>
    /// Gets the value representing "created" status.
    /// </summary>
    ///
    /// <value>
    /// Represents "created" status.
    /// </value>
    public static readonly ContactExportStatus Created = Define("created");

    /// <summary>
    /// Gets the value representing "started" status.
    /// </summary>
    ///
    /// <value>
    /// Represents "started" status.
    /// </value>
    public static readonly ContactExportStatus Started = Define("started");

    /// <summary>
    /// Gets the value representing "finished" status.
    /// </summary>
    ///
    /// <value>
    /// Represents "finished" status.
    /// </value>
    public static readonly ContactExportStatus Finished = Define("finished");
}
