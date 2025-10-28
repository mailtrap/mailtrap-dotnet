namespace Mailtrap.UnitTests.Emails.Models;


[TestFixture]
internal sealed class AttachmentValidatorTests
{
    private string _testContent { get; } = "Test content";
    private string _testFileName { get; } = "test.txt";



    #region Content

    [Test]
    public void Validation_Should_Pass_WhenContentIsNotNullOrEmpty()
    {
        var attachment = new Attachment(_testContent, _testFileName);

        var result = AttachmentValidator.Instance.TestValidate(attachment);

        result.ShouldNotHaveValidationErrorFor(a => a.Content);
    }

    #endregion



    #region FileName

    [Test]
    public void Validation_Should_Pass_WhenFileNameIsNotNullOrEmpty()
    {
        var attachment = new Attachment(_testContent, _testFileName);

        var result = AttachmentValidator.Instance.TestValidate(attachment);

        result.ShouldNotHaveValidationErrorFor(a => a.FileName);
    }

    #endregion



    #region MimeType

    [Test]
    public void Validation_Should_Pass_WhenMimeTypeIsNull()
    {
        var attachment = new Attachment(_testContent, _testFileName, DispositionType.Attachment, null);

        var result = AttachmentValidator.Instance.TestValidate(attachment);

        result.ShouldNotHaveValidationErrorFor(a => a.MimeType);
    }

    [Test]
    public void Validation_Should_Fail_WhenMimeTypeIsEmpty()
    {
        var attachment = new Attachment(_testContent, _testFileName, DispositionType.Attachment, string.Empty);

        var result = AttachmentValidator.Instance.TestValidate(attachment);

        result.ShouldHaveValidationErrorFor(a => a.MimeType);
    }

    [Test]
    public void Validation_Should_Pass_WhenMimeTypeIsNotNullOrEmpty()
    {
        var attachment = new Attachment(_testContent, _testFileName, DispositionType.Attachment, MediaTypeNames.Text.Plain);

        var result = AttachmentValidator.Instance.TestValidate(attachment);

        result.ShouldNotHaveValidationErrorFor(a => a.MimeType);
    }

    #endregion
}
