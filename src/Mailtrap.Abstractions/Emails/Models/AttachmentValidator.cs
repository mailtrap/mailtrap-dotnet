namespace Mailtrap.Emails.Models;


internal sealed class AttachmentValidator : AbstractValidator<Attachment>
{
    public static AttachmentValidator Instance { get; } = new();

    public AttachmentValidator()
    {
        RuleFor(a => a)
            .NotNull();

        RuleFor(a => a.Content)
            .NotEmpty();

        RuleFor(a => a.FileName)
            .NotEmpty();

        RuleFor(a => a.MimeType)
            .MinimumLength(1)
            .When(a => a.MimeType is not null);
    }
}
