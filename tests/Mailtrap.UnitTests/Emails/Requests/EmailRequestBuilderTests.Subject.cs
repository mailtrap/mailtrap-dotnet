namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture(TestOf = typeof(EmailRequestBuilder))]
internal sealed class EmailRequestBuilderTests_Subject
{
    private string _subject { get; } = "Subject";


    [Test]
    public void Subject_Should_ThrowArgumentNullException_WhenRequestIsNull()
    {
        var act = () => EmailRequestBuilder.Subject<EmailRequest>(null!, _subject);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Subject_Should_ThrowArgumentNullException_WhenSubjectIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.Subject(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Subject_Should_ThrowArgumentNullException_WhenSubjectIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.Subject(string.Empty);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Subject_Should_AssignSubjectProperly()
    {
        var request = EmailRequest
            .Create()
            .Subject(_subject);

        request.Subject.Should().BeSameAs(_subject);
    }

    [Test]
    public void Subject_Should_OverrideSubject_WhenCalledSeveralTimes()
    {
        var otherSubject = "Updated subject";

        var request = EmailRequest
            .Create()
            .Subject(_subject)
            .Subject(otherSubject);

        request.Subject.Should().BeSameAs(otherSubject);
    }
}
