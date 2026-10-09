namespace Mailtrap.Core.Constants;


internal static class UrlSegments
{
    internal static string BillingSegment { get; } = "billing";
    internal static string PermissionsSegment { get; } = "permissions";
    internal static string AccessesSegment { get; } = "account_accesses";
    internal static string SendingDomainsSegment { get; } = "sending_domains";
    internal static string DomainsSegment { get; } = "domains";
    internal static string CompanyInfoSegment { get; } = "company_info";

    internal static string ApiRootSegment { get; } = "api";
    internal static string ProjectsSegment { get; } = "projects";
    internal static string InboxesSegment { get; } = "inboxes";
    internal static string ContactsSegment { get; } = "contacts";
    internal static string EmailTemplatesSegment { get; } = "email_templates";
    internal static string TemplatesSegment { get; } = "templates";
    internal static string SuppressionsSegment { get; } = "suppressions";
    internal static string TrackingOptOutsSegment { get; } = "tracking_opt_outs";
    internal static string StatsSegment { get; } = "stats";
    internal static string EmailLogsSegment { get; } = "email_logs";
    internal static string ApiTokensSegment { get; } = "api_tokens";
    internal static string OrganizationsSegment { get; } = "organizations";
    internal static string SubAccountsSegment { get; } = "sub_accounts";
    internal static string WebhooksSegment { get; } = "webhooks";

    internal static string InboundSegment { get; } = "inbound";
    internal static string FoldersSegment { get; } = "folders";
    internal static string MessagesSegment { get; } = "messages";
    internal static string ThreadsSegment { get; } = "threads";
    internal static string ReplySegment { get; } = "reply";
    internal static string ReplyAllSegment { get; } = "reply_all";
    internal static string ForwardSegment { get; } = "forward";
    internal static string ForwardRulesSegment { get; } = "forward_rules";

    internal static string EmailCampaignsSegment { get; } = "email_campaigns";
    internal static string StartSegment { get; } = "start";
    internal static string ScheduleSegment { get; } = "schedule";
    internal static string CancelSegment { get; } = "cancel";
    internal static string TerminateSegment { get; } = "terminate";
    internal static string ResetSegment { get; } = "reset";
}
