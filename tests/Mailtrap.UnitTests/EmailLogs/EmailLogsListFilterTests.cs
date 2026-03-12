namespace Mailtrap.UnitTests.EmailLogs;


[TestFixture]
internal sealed class EmailLogsListFilterTests
{
    [Test]
    public void ToQueryParameters_ShouldReturnEmpty_WhenFilterIsEmpty()
    {
        var filter = new EmailLogsListFilter();
        var parameters = filter.ToQueryParameters().ToList();

        parameters.Should().BeEmpty();
    }

    [Test]
    public void ToQueryParameters_ShouldEmitSentAfterAndSentBefore()
    {
        var filter = new EmailLogsListFilter
        {
            SentAfter = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
            SentBefore = new DateTimeOffset(2025, 1, 31, 23, 59, 59, TimeSpan.Zero)
        };

        var parameters = filter.ToQueryParameters().ToList();

        parameters.Should().Contain(p => p.Key == "filters[sent_after]" && p.Value.Contains("2025-01-01"));
        parameters.Should().Contain(p => p.Key == "filters[sent_before]" && p.Value.Contains("2025-01-31"));
    }

    [Test]
    public void ToQueryParameters_ShouldEmitClauseWithOperatorAndSingleValue()
    {
        var filter = new EmailLogsListFilter
        {
            To = new EmailLogCiStringFilter { Operator = EmailLogFilterOperatorCi.CiEqual, Value = "recipient@example.com" }
        };

        var parameters = filter.ToQueryParameters().ToList();

        parameters.Should().Contain(p => p.Key == "filters[to][operator]" && p.Value == "ci_equal");
        parameters.Should().Contain(p => p.Key == "filters[to][value]" && p.Value == "recipient@example.com");
    }

    [Test]
    public void ToQueryParameters_ShouldEmitMultipleValues_WhenToFilterValuesIsSet()
    {
        var filter = new EmailLogsListFilter
        {
            To = new EmailLogCiStringFilter
            {
                Operator = EmailLogFilterOperatorCi.CiEqual,
                Values = { "a@example.com", "b@example.com" }
            }
        };

        var parameters = filter.ToQueryParameters().ToList();

        parameters.Should().Contain(p => p.Key == "filters[to][operator]" && p.Value == "ci_equal");
        parameters.Should().Contain(p => p.Key == "filters[to][value][]" && p.Value == "a@example.com");
        parameters.Should().Contain(p => p.Key == "filters[to][value][]" && p.Value == "b@example.com");
    }

    [Test]
    public void ToQueryParameters_ShouldEmitMultipleValues_WhenValuesIsSet()
    {
        var filter = new EmailLogsListFilter
        {
            Status = new EmailLogStatusFilter
            {
                Operator = EmailLogFilterOperatorEqualNotEqual.Equal,
                Values = { EmailLogStatus.Delivered, EmailLogStatus.Enqueued }
            }
        };

        var parameters = filter.ToQueryParameters().ToList();

        parameters.Should().Contain(p => p.Key == "filters[status][operator]" && p.Value == "equal");
        parameters.Should().Contain(p => p.Key == "filters[status][value][]" && p.Value == "delivered");
        parameters.Should().Contain(p => p.Key == "filters[status][value][]" && p.Value == "enqueued");
    }

    [Test]
    public void ToQueryParameters_ShouldEmitIntegerValue_ForNumberFilter()
    {
        var filter = new EmailLogsListFilter
        {
            ClicksCount = new EmailLogNumberFilter { Operator = EmailLogFilterOperatorNumber.GreaterThan, Value = 1 }
        };

        var parameters = filter.ToQueryParameters().ToList();

        parameters.Should().Contain(p => p.Key == "filters[clicks_count][operator]" && p.Value == "greater_than");
        parameters.Should().Contain(p => p.Key == "filters[clicks_count][value]" && p.Value == "1");
    }

    [Test]
    public void ToQueryParameters_ShouldEmitLongValue_ForSendingDomainIdFilter()
    {
        const long domainId = 2147483648L; // > Int32.MaxValue
        var filter = new EmailLogsListFilter
        {
            SendingDomainId = new EmailLogSendingDomainIdFilter { Operator = EmailLogFilterOperatorEqualNotEqual.Equal, Value = domainId }
        };

        var parameters = filter.ToQueryParameters().ToList();

        parameters.Should().Contain(p => p.Key == "filters[sending_domain_id][operator]" && p.Value == "equal");
        parameters.Should().Contain(p => p.Key == "filters[sending_domain_id][value]" && p.Value == "2147483648");
    }
}
