namespace Mailtrap.ApiTokens.Validators;


internal sealed class ApiTokenAccessRequestValidator : AbstractValidator<ApiTokenAccessRequest>
{
    public static ApiTokenAccessRequestValidator Instance { get; } = new();

    public ApiTokenAccessRequestValidator()
    {
        RuleFor(r => r.ResourceType)
            .NotEmpty()
            .NotEqual(ResourceType.None);

        RuleFor(r => r.ResourceId)
            .GreaterThan(0);

        RuleFor(r => r.AccessLevel)
            .NotEmpty()
            .IsInEnum()
            .Must(l => l is AccessLevel.Admin or AccessLevel.Viewer);
    }
}
