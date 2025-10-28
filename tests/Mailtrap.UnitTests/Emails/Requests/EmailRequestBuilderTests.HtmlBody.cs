namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture(TestOf = typeof(EmailRequestBuilder))]
internal sealed class EmailRequestBuilderTests_HtmlBody
{
    private string _html { get; } = "<h1>Header</h1><p>Greetings!</p>";


    [Test]
    public void Html_Should_ThrowArgumentNullException_WhenRequestIsNull()
    {
        var act = () => EmailRequestBuilder.Html<EmailRequest>(null!, _html);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Html_Should_NotThrowException_WhenHtmlIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.Html(null!);

        act.Should().NotThrow();
    }

    [Test]
    public void Html_Should_NotThrowException_WhenHtmlIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.Html(string.Empty);

        act.Should().NotThrow();
        request.HtmlBody.Should().BeEmpty();
    }

    [Test]
    public void Html_Should_AssignHtmlBodyProperly()
    {
        var request = EmailRequest
            .Create()
            .Html(_html);

        request.HtmlBody.Should().BeSameAs(_html);
    }

    [Test]
    public void Html_Should_OverrideHtmlBody_WhenCalledSeveralTimes()
    {
        var otherHtml = "<h2>Header</h2><p>Congratulation!</p>";

        var request = EmailRequest
            .Create()
            .Html(_html)
            .Html(otherHtml);

        request.HtmlBody.Should().BeSameAs(otherHtml);
    }
}
