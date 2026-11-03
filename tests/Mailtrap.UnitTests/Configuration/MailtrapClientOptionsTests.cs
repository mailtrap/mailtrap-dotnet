namespace Mailtrap.UnitTests.Configuration;


[TestFixture]
internal sealed class MailtrapClientOptionsTests
{
    [Test]
    public void Default_ShouldReturnValidDefaults()
    {
        var options = MailtrapClientOptions.Default;

        options.ApiToken.Should().BeEmpty();
        options.PrettyJson.Should().BeFalse();
        options.UseBulkApi.Should().BeFalse();
        options.SandboxId.Should().BeNull();
    }

    [Test]
    public void Default_ShouldReturnNewObjectEveryTime_WhenCalled()
    {
        var options1 = MailtrapClientOptions.Default;
        var options2 = MailtrapClientOptions.Default;

        options1.Should().NotBeSameAs(options2);
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenApiTokenIsNull()
    {
        var act = () => new MailtrapClientOptions(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenApiTokenIsEmpty()
    {
        var act = () => new MailtrapClientOptions(string.Empty);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldAssignApiTokenSettings()
    {
        var token = "token";

        var options = new MailtrapClientOptions(token);

        options.ApiToken.Should().Be(token);
    }

    [Test]
    public void Init_ShouldThrowArgumentNullException_WhenSourceIsNull()
    {
        var act = () => MailtrapClientOptions.Default.Init(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Init_ShouldCopyFieldsFromSource()
    {
        var source = MailtrapClientOptions.Default;
        var target = new MailtrapClientOptions("token");

        target.Init(source);

        target.Should().BeEquivalentTo(source);
        target.ApiToken.Should().BeSameAs(source.ApiToken);
        target.PrettyJson.Should().Be(source.PrettyJson);
        target.UseBulkApi.Should().Be(source.UseBulkApi);
        target.SandboxId.Should().Be(source.SandboxId);
    }

    [TestCase("SandboxId")]
    [TestCase("InboxId")]
    public void Bind_ShouldSetSandboxId_FromConfigurationKey(string key)
    {
        var options = BindOptions(new() { [key] = "1234" });

        options.SandboxId.Should().Be(1234);
    }

    [Test]
    public void Bind_ShouldPreferSandboxId_WhenConfigurationHasBothKeys()
    {
        var options = BindOptions(new() { ["SandboxId"] = "1234", ["InboxId"] = "5678" });

        options.SandboxId.Should().Be(1234);
    }

    [Test]
    public void ToJsonSerializerOptions_ShouldProduceOptionsBasedOnDefault([Values] bool prettyJson)
    {
        var options = new MailtrapClientOptions()
        {
            PrettyJson = prettyJson
        }.ToJsonSerializerOptions();

        var expected = new JsonSerializerOptions(MailtrapJsonSerializerOptions.Default)
        {
            WriteIndented = prettyJson
        };

        options.Should().BeEquivalentTo(expected);
    }


    private static MailtrapClientOptions BindOptions(Dictionary<string, string?> settings)
        => new ConfigurationBuilder().AddInMemoryCollection(settings).Build().Get<MailtrapClientOptions>()!;
}
