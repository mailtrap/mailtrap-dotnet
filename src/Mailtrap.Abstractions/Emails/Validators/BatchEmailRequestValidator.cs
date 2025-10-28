namespace Mailtrap.Emails.Validators;


/// <summary>
/// Represents validator for <see cref="BatchEmailRequest"/>.
/// Ensures that Requests is not empty and does not contain more than 500 items.
/// Also validates Base request if it is set and each individual request after merging with Base.
/// </summary>
internal sealed class BatchEmailRequestValidator : AbstractValidator<BatchEmailRequest>
{
    public static BatchEmailRequestValidator Instance { get; } = new();

    public BatchEmailRequestValidator()
    {
        RuleFor(r => r.Base)
            .SetValidator(EmailRequestValidator.BaseInstance)
            .When(r => r.Base is not null);

        RuleFor(r => r.Requests)
            .NotEmpty()
            .WithMessage("'Requests' must not be empty.");

        RuleFor(r => r.Requests.Count)
            .LessThanOrEqualTo(500)
            .WithMessage("'Requests' shouldn't exceed 500 records.")
            .When(r => r.Requests is not null);


        RuleForEach(r => r.GetMergedRequests())
            .NotNull()
            .SetValidator(SendEmailRequestValidator.Instance)
            .OverridePropertyName("Requests");
    }
}
