namespace Mailtrap.Webhooks.Validators;


/// <summary>
/// Validator for <see cref="CreateWebhookRequest"/>.<br/>
/// Ensures Url is an absolute URI, WebhookType is set,
/// and that <c>email_sending</c> webhooks specify SendingStream and EventTypes.
/// </summary>
public sealed class CreateWebhookRequestValidator : AbstractValidator<CreateWebhookRequest>
{
    /// <summary>
    /// Static validator instance for reuse.
    /// </summary>
    public static CreateWebhookRequestValidator Instance { get; } = new();

    /// <summary>
    /// Primary constructor.
    /// </summary>
    public CreateWebhookRequestValidator()
    {
        RuleFor(r => r.Url)
            .NotNull()
            .Must(u => u is { IsAbsoluteUri: true })
            .WithMessage("'Url' must be an absolute URI.");

        RuleFor(r => r.WebhookType)
            .NotNull()
            .Must(t => t != WebhookType.None && t != WebhookType.Unknown)
            .WithMessage("'WebhookType' must be set to a valid value.");

        When(r => r.WebhookType == WebhookType.EmailSending, () =>
        {
            RuleFor(r => r.SendingStream)
                .NotNull()
                .WithMessage("'SendingStream' is required for 'email_sending' webhook type.");

            RuleFor(r => r.EventTypes)
                .Must(events => events is { Count: > 0 })
                .WithMessage("'EventTypes' must contain at least one event for 'email_sending' webhook type.");
        });
    }
}
