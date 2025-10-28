namespace Mailtrap.AccountAccesses.Requests;


internal sealed class UpdatePermissionsRequestItemValidator : AbstractValidator<UpdatePermissionsRequestItem>
{
    public static UpdatePermissionsRequestItemValidator Instance { get; } = new();

    public UpdatePermissionsRequestItemValidator()
    {
        RuleFor(r => r.ResourceId)
            .GreaterThan(0);

        RuleFor(r => r.ResourceType)
            .NotEmpty()
            .NotEqual(ResourceType.None);

        RuleFor(r => r.AccessLevel)
            .NotEmpty()
            .IsInEnum()
            .Must(l => l is AccessLevel.Admin or AccessLevel.Viewer);
    }
}
