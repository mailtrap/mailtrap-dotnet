namespace Mailtrap.ContactExports.Validators;


/// <summary>
/// Validator for <see cref="CreateContactExportRequest"/>
/// Enforces limits on the number of filters and ensures each filter is valid.
/// </summary>
internal sealed class CreateContactExportRequestValidator : AbstractValidator<CreateContactExportRequest>
{
    public const int MaxFiltersPerRequest = 50_000;
    public const int MinFiltersPerRequest = 1;

    /// <summary>
    /// Static validator instance for reuse.
    /// </summary>
    public static CreateContactExportRequestValidator Instance { get; } = new();

    /// <summary>
    /// Primary constructor.
    /// </summary>
    public CreateContactExportRequestValidator()
    {
        RuleFor(r => r.Filters)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must(list => list.Count is >= MinFiltersPerRequest and <= MaxFiltersPerRequest);

        RuleForEach(r => r.Filters)
            .NotNull()
            .SetValidator(ContactExportFilterValidator.Instance);
    }
}
