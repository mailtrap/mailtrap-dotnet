namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture(TestOf = typeof(EmailRequestBuilder))]
internal sealed class EmailRequestBuilderTests_TextBody
{
    private string _text { get; } = "Some text";


    [Test]
    public void Text_Should_ThrowArgumentNullException_WhenRequestIsNull()
    {
        var act = () => EmailRequestBuilder.Text<EmailRequest>(null!, _text);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Text_Should_NotThrowException_WhenTextIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.Text(null!);

        act.Should().NotThrow();
    }

    [Test]
    public void Text_Should_NotThrowException_WhenTextIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.Text(string.Empty);

        act.Should().NotThrow();
    }

    [Test]
    public void Text_Should_AssignTextBodyProperly()
    {
        var request = EmailRequest
            .Create()
            .Text(_text);

        request.TextBody.Should().Be(_text);
    }

    [Test]
    public void Text_Should_OverrideTextBody_WhenCalledSeveralTimes()
    {
        var otherText = "Updated Text";

        var request = EmailRequest
            .Create()
            .Text(_text)
            .Text(otherText);

        request.TextBody.Should().Be(otherText);
    }
}
