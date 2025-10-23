namespace Mailtrap.ContactExports.Validators;


/// <summary>
/// Validator for <see cref="ContactExportFilterBase"/> and its derived types:
/// <see cref="ContactExportListIdFilter"/> and <see cref="ContactExportSubscriptionStatusFilter"/>.
/// Ensures that filter is fully specified according to its type.
/// </summary>
internal sealed class ContactExportFilterValidator : AbstractValidator<ContactExportFilterBase>
{
    /// <summary>
    /// Static validator instance for reuse.
    /// </summary>
    public static ContactExportFilterValidator Instance { get; } = new();

    private ContactExportFilterValidator()
    {
        // Common validation for all filters

        RuleFor(f => f.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Must(ContactExportFilterName.IsDefined);

        RuleFor(f => f.Operator)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Must(ContactExportFilterOperator.IsDefined);

        // Conditional validation based on runtime type

        When(f => f is ContactExportListIdFilter, () =>
        {
            RuleFor(f => (f as ContactExportListIdFilter)!.Value)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty();
        });

        When(f => f is ContactExportSubscriptionStatusFilter, () =>
        {
            RuleFor(f => (f as ContactExportSubscriptionStatusFilter)!.Value)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .Must(ContactExportFilterSubscriptionStatus.IsDefined);
        });
    }
}
