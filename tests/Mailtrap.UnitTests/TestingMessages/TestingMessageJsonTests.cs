using Mailtrap.TestingMessages.Models;

namespace Mailtrap.UnitTests.TestingMessages;

/// <summary>
/// Regression tests for JSON deserialization of <see cref="TestingMessage"/>.
/// </summary>
[TestFixture]
internal sealed class TestingMessageJsonTests
{
    [Test]
    public void Deserialize_ShouldHandleBlacklistsReportInfoWithResultError()
    {
        const string json = """
            {
                "id": 1,
                "inbox_id": 2,
                "blacklists_report_info": {"result": "error"},
                "smtp_information": {"ok": true, "data": {}}
            }
            """;

        var message = JsonSerializer.Deserialize<TestingMessage>(json);

        message.Should().NotBeNull();
        message!.BlacklistsReportInfo.Result.Should().Be(BlacklistReportResult.Error);
    }

    [Test]
    public void Deserialize_ShouldTreatBlacklistsReportInfoBooleanAsEmptyReport()
    {
        const string json = """
            {
                "id": 1,
                "inbox_id": 2,
                "blacklists_report_info": false,
                "smtp_information": {"ok": true, "data": {}}
            }
            """;

        var message = JsonSerializer.Deserialize<TestingMessage>(json);

        message.Should().NotBeNull();
        message!.BlacklistsReportInfo.Result.Should().Be(BlacklistReportResult.Unknown);
    }
}
