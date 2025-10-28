namespace Mailtrap.UnitTests.Emails.Requests;


[TestFixture(TestOf = typeof(EmailRequestBuilder))]
internal sealed class EmailRequestBuilderTests_Attachment
{
    private string Content { get; } = "Attachment Content";
    private string FileName { get; } = "filename.ext";
    private Attachment _attachment1 { get; } = new("Content 1", "file1.txt");
    private Attachment _attachment2 { get; } = new("Content 2", "file2.txt", mimeType: MediaTypeNames.Text.Plain, contentId: "ID2");



    #region Attach

    [Test]
    public void Attach_Should_ThrowArgumentNullException_WhenRequestIsNull()
    {
        var act = () => EmailRequestBuilder.Attach<EmailRequest>(null!, _attachment1);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Attach_Should_ThrowArgumentNullException_WhenParamsIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.Attach(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Attach_Should_NotThrowException_WhenParamsIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.Attach([]);

        act.Should().NotThrow();
    }

    [Test]
    public void Attach_Should_AddAttachmentsToCollection()
    {
        Attach_CreateAndValidate(_attachment1, _attachment2);
    }

    [Test]
    public void Attach_Should_AddAttachmentsToCollection_WhenCalledMultipleTimes()
    {
        var attachment3 = new Attachment("Content 3", "file3.png", DispositionType.Inline, MediaTypeNames.Image.Png);
        var attachment4 = new Attachment("Content 4", "file4.pdf", mimeType: MediaTypeNames.Application.Pdf, contentId: "ID 4");

        var request = Attach_CreateAndValidate(_attachment1, _attachment2);

        request.Attach(attachment3, attachment4);

        request.Attachments.Should()
            .HaveCount(4).And
            .ContainInOrder(_attachment1, _attachment2, attachment3, attachment4);
    }

    [Test]
    public void Attach_Should_NotAddAttachmentsToCollection_WhenParamsIsEmpty()
    {
        var request = Attach_CreateAndValidate(_attachment1, _attachment2);

        request.Attach([]);

        request.Attachments.Should()
            .HaveCount(2).And
            .ContainInOrder(_attachment1, _attachment2);
    }


    private static EmailRequest Attach_CreateAndValidate(params Attachment[] attachments)
    {
        var request = EmailRequest
            .Create()
            .Attach(attachments);

        request.Attachments.Should()
            .HaveCount(attachments.Length).And
            .ContainInOrder(attachments);

        return request;
    }

    #endregion



    #region Attach(content, fileName, disposition, mimeType, contentId)

    [Test]
    public void Attach_Should_ThrowArgumentNullException_WhenRequestIsNull_2()
    {
        var act = () => EmailRequestBuilder.Attach<EmailRequest>(null!, Content, FileName);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Attach_Should_ThrowArgumentNullException_WhenContentIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.Attach(null!, FileName);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Attach_Should_ThrowArgumentNullException_WhenContentIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.Attach(string.Empty, FileName);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Attach_Should_ThrowArgumentNullException_WhenFileNameIsNull()
    {
        var request = EmailRequest.Create();

        var act = () => request.Attach(Content, null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Attach_Should_ThrowArgumentNullException_WhenFileNameIsEmpty()
    {
        var request = EmailRequest.Create();

        var act = () => request.Attach(Content, string.Empty);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test, Combinatorial]
    public void Attach_Should_NotThrowException_WhenAllOptionalParamsAreNullOrEmpty(
        [Values(null, "")] string? mimeType,
        [Values(null, "")] string? contentId)
    {
        var request = EmailRequest.Create();

        var act = () => request.Attach(Content, FileName, null, mimeType, contentId);

        act.Should().NotThrow();
    }

    [Test, Combinatorial]
    public void Attach_Should_NotThrowException_WhenDispositionIsSetAndOtherOptionalParamsAreNullOrEmpty(
        [Values(null, "")] string? mimeType,
        [Values(null, "")] string? contentId)
    {
        var request = EmailRequest.Create();

        var act = () => request.Attach(Content, FileName, DispositionType.Inline, mimeType, contentId);

        act.Should().NotThrow();
    }

    [Test]
    public void Attach_Should_AddAttachmentToCollectionAndInitPropertiesCorrectly_WhenOnlyRequiredParametersSpecified()
    {
        var request = EmailRequest
            .Create()
            .Attach(Content, FileName);

        request.Attachments.Should().ContainSingle();

        var added = request.Attachments.First();
        added.Content.Should().Be(Content);
        added.FileName.Should().Be(FileName);
        added.Disposition.Should().Be(DispositionType.Attachment);
        added.MimeType.Should().BeNull();
        added.ContentId.Should().BeNull();
    }

    [Test]
    public void Attach_Should_AddAttachmentToCollectionAndInitPropertiesCorrectly_WhenAllParametersSpecified()
    {
        var disposition = DispositionType.Inline;
        var mimeType = MediaTypeNames.Image.Png;
        var contentId = "<ID>";

        var request = EmailRequest
            .Create()
            .Attach(Content, FileName, disposition, mimeType, contentId);

        request.Attachments.Should().ContainSingle();

        var added = request.Attachments.First();
        added.Content.Should().Be(Content);
        added.FileName.Should().Be(FileName);
        added.Disposition.Should().Be(disposition);
        added.MimeType.Should().Be(mimeType);
        added.ContentId.Should().Be(contentId);
    }

    #endregion
}
