using System.Globalization;


namespace Mailtrap.EmailLogs.Models;


/// <summary>
/// Filter for listing email logs. Uses typed filter fields per spec (concrete operators per field).
/// </summary>
public sealed record EmailLogsListFilter
{
    /// <summary>
    /// Start of sent-at range (ISO 8601). Must be before or equal to <see cref="SentBefore"/>.
    /// </summary>
    public DateTimeOffset? SentAfter { get; set; }

    /// <summary>
    /// End of sent-at range (ISO 8601). Must be after or equal to <see cref="SentAfter"/>.
    /// </summary>
    public DateTimeOffset? SentBefore { get; set; }

    /// <summary>
    /// Filter by recipient (to) address. Operators: ci_contain, ci_not_contain, ci_equal, ci_not_equal.
    /// </summary>
    public EmailLogCiStringFilter? To { get; set; }

    /// <summary>
    /// Filter by sender (from) address. Operators: ci_contain, ci_not_contain, ci_equal, ci_not_equal.
    /// </summary>
    public EmailLogCiStringFilter? From { get; set; }

    /// <summary>
    /// Filter by subject. Operators: ci_contain, ci_not_contain, ci_equal, ci_not_equal, empty, not_empty.
    /// </summary>
    public EmailLogCiStringOptionalFilter? Subject { get; set; }

    /// <summary>
    /// Filter by message status. Operators: equal, not_equal. Values: delivered, not_delivered, enqueued, opted_out.
    /// </summary>
    public EmailLogStatusFilter? Status { get; set; }

    /// <summary>
    /// Filter by events. Operators: include_event, not_include_event. Values: delivery, open, click, bounce, spam, unsubscribe, soft_bounce, reject, suspension.
    /// </summary>
    public EmailLogEventsFilter? Events { get; set; }

    /// <summary>
    /// Filter by clicks count. Operators: equal, greater_than, less_than.
    /// </summary>
    public EmailLogNumberFilter? ClicksCount { get; set; }

    /// <summary>
    /// Filter by opens count. Operators: equal, greater_than, less_than.
    /// </summary>
    public EmailLogNumberFilter? OpensCount { get; set; }

    /// <summary>
    /// Filter by client IP. Operators: equal, not_equal, contain, not_contain.
    /// </summary>
    public EmailLogStringFilter? ClientIp { get; set; }

    /// <summary>
    /// Filter by sending IP. Operators: equal, not_equal, contain, not_contain.
    /// </summary>
    public EmailLogStringFilter? SendingIp { get; set; }

    /// <summary>
    /// Filter by email service provider response. Operators: ci_contain, ci_not_contain, ci_equal, ci_not_equal.
    /// </summary>
    public EmailLogCiStringFilter? EmailServiceProviderResponse { get; set; }

    /// <summary>
    /// Filter by email service provider. Operators: equal, not_equal.
    /// </summary>
    public EmailLogExactStringFilter? EmailServiceProvider { get; set; }

    /// <summary>
    /// Filter by recipient MX. Operators: ci_contain, ci_not_contain, ci_equal, ci_not_equal.
    /// </summary>
    public EmailLogCiStringFilter? RecipientMx { get; set; }

    /// <summary>
    /// Filter by category. Operators: equal, not_equal.
    /// </summary>
    public EmailLogExactStringFilter? Category { get; set; }

    /// <summary>
    /// Filter by sending domain ID. Operators: equal, not_equal.
    /// </summary>
    public EmailLogSendingDomainIdFilter? SendingDomainId { get; set; }

    /// <summary>
    /// Filter by sending stream. Operators: equal, not_equal. Values: transactional, bulk.
    /// </summary>
    public EmailLogSendingStreamFilter? SendingStream { get; set; }

    /// <summary>
    /// Produces query key-value pairs for the filters prefix.
    /// </summary>
    public IEnumerable<KeyValuePair<string, string>> ToQueryParameters()
    {
        const string prefix = "filters";

        if (SentAfter.HasValue)
        {
            var value = SentAfter.Value.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
            yield return new KeyValuePair<string, string>($"{prefix}[sent_after]", value);
        }

        if (SentBefore.HasValue)
        {
            var value = SentBefore.Value.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
            yield return new KeyValuePair<string, string>($"{prefix}[sent_before]", value);
        }

        var filterDescriptors = new (string Field, IEmailLogFilterField? Filter)[]
        {
            ("to", To),
            ("from", From),
            ("subject", Subject),
            ("status", Status),
            ("events", Events),
            ("clicks_count", ClicksCount),
            ("opens_count", OpensCount),
            ("client_ip", ClientIp),
            ("sending_ip", SendingIp),
            ("email_service_provider_response", EmailServiceProviderResponse),
            ("email_service_provider", EmailServiceProvider),
            ("recipient_mx", RecipientMx),
            ("category", Category),
            ("sending_domain_id", SendingDomainId),
            ("sending_stream", SendingStream),
        };

        foreach (var (field, filter) in filterDescriptors)
        {
            foreach (var pair in Emit(prefix, field, filter))
            {
                yield return pair;
            }
        }
    }

    private static IEnumerable<KeyValuePair<string, string>> Emit(string prefix, string field, IEmailLogFilterField? filter)
    {
        if (filter is null)
        {
            yield break;
        }

        foreach (var pair in filter.ToQueryParameters(prefix, field))
        {
            yield return pair;
        }
    }
}
