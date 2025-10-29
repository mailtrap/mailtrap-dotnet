namespace Mailtrap.Core.Constants;


internal static class UrlSegments
{
    internal static string BillingSegment { get; } = "billing";
    internal static string PermissionsSegment { get; } = "permissions";
    internal static string AccessesSegment { get; } = "account_accesses";
    internal static string SendingDomainsSegment { get; } = "sending_domains";

    internal static string ApiRootSegment { get; } = "api";
    internal static string ProjectsSegment { get; } = "projects";
    internal static string InboxesSegment { get; } = "inboxes";
    internal static string ContactsSegment { get; } = "contacts";
    internal static string EmailTemplatesSegment { get; } = "email_templates";
    internal static string SuppressionsSegment { get; } = "suppressions";
}
