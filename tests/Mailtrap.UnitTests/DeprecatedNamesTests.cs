// The deprecated names below are [Obsolete] on purpose: these tests call them to prove they still work.
#pragma warning disable CS0618

using System.Reflection;


namespace Mailtrap.UnitTests;


[TestFixture]
internal sealed class DeprecatedNamesTests
{
    private readonly IRestResourceCommandFactory _commandFactoryMock = Mock.Of<IRestResourceCommandFactory>();
    private readonly long _id = TestContext.CurrentContext.Random.NextLong(1, long.MaxValue);


    #region Obsolete attribute

    [TestCase(typeof(IMailtrapClient), "Test(long)", "Sandbox(long)")]
    [TestCase(typeof(IMailtrapClient), "BatchTest(long)", "BatchSandbox(long)")]
    [TestCase(typeof(MailtrapClient), "Test(long)", "Sandbox(long)")]
    [TestCase(typeof(MailtrapClient), "BatchTest(long)", "BatchSandbox(long)")]
    [TestCase(typeof(IAccountResource), "Inboxes()", "Sandboxes()")]
    [TestCase(typeof(IAccountResource), "Inbox(long)", "Sandbox(long)")]
    [TestCase(typeof(IAccountResource), "SendingDomains()", "Domains()")]
    [TestCase(typeof(IAccountResource), "SendingDomain(long)", "Domain(long)")]
    [TestCase(typeof(AccountResource), "Inboxes()", "Sandboxes()")]
    [TestCase(typeof(AccountResource), "Inbox(long)", "Sandbox(long)")]
    [TestCase(typeof(AccountResource), "SendingDomains()", "Domains()")]
    [TestCase(typeof(AccountResource), "SendingDomain(long)", "Domain(long)")]
    [TestCase(typeof(MailtrapClientOptions), "InboxId", "SandboxId")]
    public void DeprecatedMember_ShouldBeObsolete_AndPointToReplacement(Type type, string oldName, string newName)
    {
        var member = type.GetMember(oldName.Split('(')[0]).Should().ContainSingle().Subject;

        var attribute = member.GetCustomAttribute<ObsoleteAttribute>();

        attribute.Should().NotBeNull();
        attribute!.IsError.Should().BeFalse();
        attribute.Message.Should().Be(
            $"{oldName} is deprecated and will be removed in a future major release; use {newName} instead.");
    }

    #endregion


    #region IMailtrapClient

    [Test]
    public void Test_ShouldReturnSameClientAsSandbox()
    {
        var client = CreateClient();

        ResourceValidator.Validate<ISendEmailClient, SendEmailClient>(
            client.Test(_id), client.Sandbox(_id).ResourceUri);
    }

    [Test]
    public void BatchTest_ShouldReturnSameClientAsBatchSandbox()
    {
        var client = CreateClient();

        ResourceValidator.Validate<IBatchEmailClient, BatchEmailClient>(
            client.BatchTest(_id), client.BatchSandbox(_id).ResourceUri);
    }

    #endregion


    #region IAccountResource

    [Test]
    public void Inboxes_ShouldReturnSameResourceAsSandboxes()
    {
        var account = CreateAccount();

        ResourceValidator.Validate<IInboxCollectionResource, InboxCollectionResource>(
            account.Inboxes(), account.Sandboxes().ResourceUri);
    }

    [Test]
    public void Inbox_ShouldReturnSameResourceAsSandbox()
    {
        var account = CreateAccount();

        ResourceValidator.Validate<IInboxResource, InboxResource>(
            account.Inbox(_id), account.Sandbox(_id).ResourceUri);
    }

    [Test]
    public void SendingDomains_ShouldReturnSameResourceAsDomains()
    {
        var account = CreateAccount();

        ResourceValidator.Validate<ISendingDomainCollectionResource, SendingDomainCollectionResource>(
            account.SendingDomains(), account.Domains().ResourceUri);
    }

    [Test]
    public void SendingDomain_ShouldReturnSameResourceAsDomain()
    {
        var account = CreateAccount();

        ResourceValidator.Validate<ISendingDomainResource, SendingDomainResource>(
            account.SendingDomain(_id), account.Domain(_id).ResourceUri);
    }

    #endregion


    #region MailtrapClientOptions

    [Test]
    public void InboxId_ShouldReadAndWriteSandboxId()
    {
        var options = new MailtrapClientOptions { InboxId = _id };

        options.SandboxId.Should().Be(_id);

        options.SandboxId = null;

        options.InboxId.Should().BeNull();
    }

    #endregion


    private MailtrapClient CreateClient()
    {
        var emailClientFactory = new EmailClientFactory(
            Options.Create(new MailtrapClientOptions("token")),
            new EmailClientEndpointProvider(),
            _commandFactoryMock);

        return new MailtrapClient(emailClientFactory, _commandFactoryMock);
    }

    private AccountResource CreateAccount()
        => new(
            _commandFactoryMock,
            EndpointsTestConstants.ApiDefaultUrl
                .Append(UrlSegmentsTestConstants.ApiRootSegment, UrlSegmentsTestConstants.AccountsSegment)
                .Append(_id));
}
