namespace Mailtrap.UnitTests.Core.Constants;


[TestFixture]
internal sealed class UrlSegmentsTests
{
    [Test]
    public void ApiRootSegment_ShouldContainCorrectValue()
    {
        UrlSegments.ApiRootSegment.Should().Be(UrlSegmentsTestConstants.ApiRootSegment);
    }

    [Test]
    public void ProjectsSegment_ShouldContainCorrectValue()
    {
        UrlSegments.ProjectsSegment.Should().Be(UrlSegmentsTestConstants.ProjectsSegment);
    }

    [Test]
    public void InboxesSegment_ShouldContainCorrectValue()
    {
        UrlSegments.InboxesSegment.Should().Be(UrlSegmentsTestConstants.InboxesSegment);
    }

    [Test]
    public void EmailLogsSegment_ShouldContainCorrectValue()
    {
        UrlSegments.EmailLogsSegment.Should().Be(UrlSegmentsTestConstants.EmailLogsSegment);
    }
}
