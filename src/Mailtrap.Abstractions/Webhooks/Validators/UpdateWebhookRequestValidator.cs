namespace Mailtrap.Webhooks.Validators;


/// <summary>
/// Validator for <see cref="UpdateWebhookRequest"/>.<br/>
/// Ensures Url, when provided, is an absolute URI.
/// </summary>
public sealed class UpdateWebhookRequestValidator : AbstractValidator<UpdateWebhookRequest>
{
    /// <summary>
    /// Static validator instance for reuse.
    /// </summary>
    public static UpdateWebhookRequestValidator Instance { get; } = new();

    /// <summary>
    /// Primary constructor.
    /// </summary>
    public UpdateWebhookRequestValidator()
    {
        When(r => r.Url is not null, () =>
        {
            RuleFor(r => r.Url)
                .Must(u => u is { IsAbsoluteUri: true })
                .WithMessage("'Url' must be an absolute URI.");
        });
    }
}
