﻿// -----------------------------------------------------------------------
// <copyright file="AttachmentTests.cs" company="Railsware Products Studio, LLC">
// Copyright (c) Railsware Products Studio, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------


namespace Mailtrap.UnitTests.Emails.Models;


[TestFixture]
internal sealed class AttachmentTests
{
    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenContentIsNull()
    {
        var act = () => new Attachment(null!, string.Empty);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenContentIsEmpty()
    {
        var act = () => new Attachment(string.Empty, string.Empty);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenFileNameIsNull()
    {
        var act = () => new Attachment("Test Text", null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenFileNameIsEmpty()
    {
        var act = () => new Attachment("Test Text", string.Empty);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldNotThrowException_WhenEmptyFileNameProvided()
    {
        var act = () => new Attachment("Test Text", "filename.txt");

        act.Should().NotThrow();
    }

    [Test]
    public void Constructor_ShouldNotThrowException_WhenDispositionIsSetToNull()
    {
        var act = () => new Attachment("Test Text", "filename.txt", null);

        act.Should().NotThrow();
    }

    [Test]
    public void Constructor_ShouldDefaultDispositionToAttachment_WhenNotSpecified()
    {
        var attachment = new Attachment("Test Text", "filename.txt");

        attachment.Disposition.Should().Be(DispositionType.Attachment);
    }

    [Test]
    public void Constructor_ShouldDefaultDispositionToAttachment_WhenIsSetToNullExplicitly()
    {
        var attachment = new Attachment("Test Text", "filename.txt", null);

        attachment.Disposition.Should().Be(DispositionType.Attachment);
    }

    [Test]
    public void Constructor_ShouldDefaultOptionalPropertiesToNull_WhenNotSpecified()
    {
        var attachment = new Attachment("Test Text", "filename.txt");

        attachment.MimeType.Should().BeNull();
        attachment.ContentId.Should().BeNull();
    }

    [Test]
    public void Constructor_ShouldAssignPropertiesCorrectly()
    {
        var content = "Content";
        var fileName = "FileName";
        var disposition = DispositionType.Inline;
        var mimeType = MediaTypeNames.Image.Png;
        var contentId = "ID";

        var attachment = new Attachment(content, fileName, disposition, mimeType, contentId);

        attachment.Content.Should().Be(content);
        attachment.FileName.Should().Be(fileName);
        attachment.Disposition.Should().Be(disposition);
        attachment.MimeType.Should().Be(mimeType);
        attachment.ContentId.Should().Be(contentId);
    }

    [Test]
    public void ShouldSerializeCorrectly()
    {
        var content = "Content";
        var fileName = "FileName";
        var disposition = DispositionType.Inline;
        var mimeType = MediaTypeNames.Image.Png;
        var contentId = "ID";

        var attachment = new Attachment(content, fileName, disposition, mimeType, contentId);

        var serialized = JsonSerializer.Serialize(attachment, MailtrapJsonSerializerOptions.NotIndented);

        // This straightforward JSON assertion is quite fragile,
        // since depends on the order of properties in the JSON string.
        // For now, [JsonPropertyOrder] attributes were added to model
        // to ensure property order in serialized JSON.
        // TODO: Find more stable way to assert JSON serialization.
        serialized.Should().Be(
            "{" +
                "content".AddDoubleQuote() + ":" + content.AddDoubleQuote() + "," +
                "filename".AddDoubleQuote() + ":" + fileName.AddDoubleQuote() + "," +
                "disposition".AddDoubleQuote() + ":" + DispositionType.Inline.ToString().AddDoubleQuote() + "," +
                "type".AddDoubleQuote() + ":" + mimeType.AddDoubleQuote() + "," +
                "content_id".AddDoubleQuote() + ":" + contentId.AddDoubleQuote() + "" +
            "}");

        var deserialized = JsonSerializer.Deserialize<Attachment>(serialized, MailtrapJsonSerializerOptions.NotIndented);

        deserialized.Should().BeEquivalentTo(attachment);
    }
}
