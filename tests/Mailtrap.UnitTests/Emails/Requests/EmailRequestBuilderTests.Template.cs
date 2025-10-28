namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture(TestOf = typeof(EmailRequestBuilder))]
internal sealed class EmailRequestBuilderTests_Template
{
    private string _templateId { get; } = "<ID>";


    [Test]
    public void Template_Should_ThrowArgumentNullException_WhenRequestIsNull()
    {
        var act = () => EmailRequestBuilder.Template<EmailRequest>(null!, _templateId);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Template_Should_ThrowArgumentNullException_WhenTemplateIdIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.Template(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Template_Should_ThrowArgumentNullException_WhenTemplateIdIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.Template(string.Empty);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Template_Should_AssignTemplateProperly()
    {
        var request = EmailRequest
            .Create()
            .Template(_templateId);

        request.TemplateId.Should().Be(_templateId);
    }

    [Test]
    public void Template_Should_OverrideTemplate_WhenCalledSeveralTimes()
    {
        var otherTemplate = "<ID2>";

        var request = EmailRequest
            .Create()
            .Template(_templateId)
            .Template(otherTemplate);

        request.TemplateId.Should().Be(otherTemplate);
    }
}
