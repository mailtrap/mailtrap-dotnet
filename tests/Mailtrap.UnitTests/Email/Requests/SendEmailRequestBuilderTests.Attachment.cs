﻿// -----------------------------------------------------------------------
// <copyright file="SendEmailRequestBuilderTests.Attachment.cs" company="Railsware Products Studio, LLC">
// Copyright (c) Railsware Products Studio, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Mailtrap.UnitTests.Email.Requests;


[TestFixture(TestOf = typeof(SendEmailRequestBuilder))]
internal sealed class SendEmailRequestBuilderTests_Attachment
{
    private string Content { get; } = "Attachment Content";
    private string FileName { get; } = "filename.ext";
    private Attachment _attachment1 { get; } = new("Content 1", "file1.txt");
    private Attachment _attachment2 { get; } = new("Content 2", "file2.txt", mimeType: MediaTypeNames.Text.Plain, contentId: "ID2");



    #region Attach

    [Test]
    public void Attach_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        var act = () => SendEmailRequestBuilder.Attach(null!, _attachment1);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Attach_ShouldThrowArgumentNullException_WhenParamsIsNull()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Attach(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Attach_ShouldNotThrowException_WhenParamsIsEmpty()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Attach([]);

        act.Should().NotThrow();
    }

    [Test]
    public void Attach_ShouldAddAttachmentsToCollection()
    {
        Attach_CreateAndValidate(_attachment1, _attachment2);
    }

    [Test]
    public void Attach_ShouldAddAttachmentsToCollection_WhenCalledMultipleTimes()
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
    public void Attach_ShouldNotAddAttachmentsToCollection_WhenParamsIsEmpty()
    {
        var request = Attach_CreateAndValidate(_attachment1, _attachment2);

        request.Attach([]);

        request.Attachments.Should()
            .HaveCount(2).And
            .ContainInOrder(_attachment1, _attachment2);
    }


    private static SendEmailRequest Attach_CreateAndValidate(params Attachment[] attachments)
    {
        var request = SendEmailRequest
            .Create()
            .Attach(attachments);

        request.Attachments.Should()
            .HaveCount(2).And
            .ContainInOrder(attachments);

        return request;
    }

    #endregion



    #region Attach(content, fileName, disposition, mimeType, contentId)

    [Test]
    public void Attach_ShouldThrowArgumentNullException_WhenRequestIsNull_2()
    {
        var request = SendEmailRequest.Create();

        var act = () => SendEmailRequestBuilder.Attach(null!, Content, FileName);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Attach_ShouldThrowArgumentNullException_WhenContentIsNull()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Attach(null!, FileName);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Attach_ShouldThrowArgumentNullException_WhenContentIsEmpty()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Attach(string.Empty, FileName);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Attach_ShouldThrowArgumentNullException_WhenFileNameIsNull()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Attach(Content, null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Attach_ShouldThrowArgumentNullException_WhenFileNameIsEmpty()
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Attach(Content, string.Empty);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test, Combinatorial]
    public void Attach_ShouldNotThrowException_WhenAllOptionalParamsAreNullOrEmpty(
        [Values(null, "")] string? mimeType,
        [Values(null, "")] string? contentId)
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Attach(Content, FileName, null, mimeType, contentId);

        act.Should().NotThrow();
    }

    [Test, Combinatorial]
    public void Attach_ShouldNotThrowException_WhenDispositionIsSetAndOtherOptionalParamsAreNullOrEmpty(
        [Values(null, "")] string? mimeType,
        [Values(null, "")] string? contentId)
    {
        var request = SendEmailRequest.Create();

        var act = () => request.Attach(Content, FileName, DispositionType.Inline, mimeType, contentId);

        act.Should().NotThrow();
    }

    [Test]
    public void Attach_ShouldAddAttachmentToCollectionAndInitPropertiesCorrectly_WhenOnlyRequiredParametersSpecified()
    {
        var request = SendEmailRequest
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
    public void Attach_ShouldAddAttachmentToCollectionAndInitPropertiesCorrectly_WhenAllParametersSpecified()
    {
        var disposition = DispositionType.Inline;
        var mimeType = MediaTypeNames.Image.Png;
        var contentId = "<ID>";

        var request = SendEmailRequest
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
