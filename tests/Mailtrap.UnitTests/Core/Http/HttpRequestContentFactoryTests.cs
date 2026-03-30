namespace Mailtrap.UnitTests.Core.Http;


[TestFixture]
internal sealed class HttpRequestContentFactoryTests
{
    private readonly IOptions<MailtrapClientOptions> _options =
        Options.Create(MailtrapClientOptions.Default);


    [Test]
    public void Create_ShouldReturnNull_WhenContentIsNull()
    {
        var factory = new HttpRequestContentFactory(_options);

        using var content = factory.CreateStringContent<object>(null);

        content.Should().BeNull();
    }

    [Test]
    public void Create_ShouldApplyHeaders()
    {
        var factory = new HttpRequestContentFactory(_options);

        using var content = factory.CreateStringContent(string.Empty);

        content.Should().NotBeNull();
        content.Headers.Should().ContainKey("Content-Type");
        content.Headers.ContentType.Should().NotBeNull();
        content.Headers.ContentType!.MediaType.Should().Be(MimeTypes.Application.Json);
    }

    [Test]
    public async Task Create_ShouldSetContentProperly()
    {
        var factory = new HttpRequestContentFactory(_options);

        var json = "content";

        using var content = factory.CreateStringContent(json);

        var result = await content!.ReadAsStringAsync();

        result.Should().Be(json.AddDoubleQuote());
    }
}
