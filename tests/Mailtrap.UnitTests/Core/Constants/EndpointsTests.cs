namespace Mailtrap.UnitTests.Core.Constants;


[TestFixture]
internal sealed class EndpointsTests
{
    [Test]
    public void ApiDefaultUrl_ShouldContainCorrectValue()
    {
        Endpoints.ApiDefaultUrl.Should().Be(EndpointsTestConstants.ApiDefaultUrl);
    }

    [Test]
    public void SendDefaultUrl_ShouldContainCorrectValue()
    {
        Endpoints.SendDefaultUrl.Should().Be(EndpointsTestConstants.SendDefaultUrl);
    }

    [Test]
    public void BulkDefaultUrl_ShouldContainCorrectValue()
    {
        Endpoints.BulkDefaultUrl.Should().Be(EndpointsTestConstants.BulkDefaultUrl);
    }

    [Test]
    public void SandboxDefaultUrl_ShouldContainCorrectValue()
    {
        Endpoints.SandboxDefaultUrl.Should().Be(EndpointsTestConstants.SandboxDefaultUrl);
    }
}
