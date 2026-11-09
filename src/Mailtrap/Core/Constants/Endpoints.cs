namespace Mailtrap.Core.Constants;


/// <summary>
/// Contains predefined default API endpoints for Mailtrap.
/// </summary>
internal static class Endpoints
{
    /// <summary>
    /// Default base URL for Mailtrap API endpoint.<br />
    /// <see href="https://api-docs.mailtrap.io/docs/mailtrap-api-docs/82708d3cc9606-general"/>
    /// </summary>
    internal static Uri ApiDefaultUrl { get; } = new("https://mailtrap.io");

    /// <summary>
    /// Default base URL for Mailtrap API send endpoint.<br />
    /// <see href="https://api-docs.mailtrap.io/docs/mailtrap-api-docs/67f1d70aeb62c-send-email-including-templates"/>
    /// </summary>
    internal static Uri SendDefaultUrl { get; } = new("https://send.api.mailtrap.io");

    /// <summary>
    /// Default base URL for Mailtrap API send endpoint.<br />
    /// <see href="https://api-docs.mailtrap.io/docs/mailtrap-api-docs/711042a37477c-send-email-including-templates"/>
    /// </summary>
    internal static Uri BulkDefaultUrl { get; } = new("https://bulk.api.mailtrap.io");

    /// <summary>
    /// Default base URL for Mailtrap API sandbox endpoint.<br />
    /// <see href="https://api-docs.mailtrap.io/docs/mailtrap-api-docs/bcf61cdc1547e-send-email-including-templates"/>
    /// </summary>
    internal static Uri SandboxDefaultUrl { get; } = new("https://sandbox.api.mailtrap.io");
}
