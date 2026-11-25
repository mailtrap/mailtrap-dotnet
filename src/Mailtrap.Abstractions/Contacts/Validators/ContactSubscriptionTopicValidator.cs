namespace Mailtrap.Contacts.Validators;


/// <summary>
/// Validator for <see cref="ContactSubscriptionTopic"/> entries of contact requests.<br />
/// Ensures the subscription topic and sending domain identifiers are positive and the status is set.
/// </summary>
public sealed class ContactSubscriptionTopicValidator : AbstractValidator<ContactSubscriptionTopic>
{
    /// <summary>
    /// Static validator instance for reuse.
    /// </summary>
    public static ContactSubscriptionTopicValidator Instance { get; } = new();

    /// <summary>
    /// Primary constructor.
    /// </summary>
    public ContactSubscriptionTopicValidator()
    {
        RuleFor(t => t.SubscriptionTopicId)
            .GreaterThan(0)
            .WithMessage("'SubscriptionTopicId' must be a positive subscription topic ID.");

        RuleFor(t => t.DomainId)
            .GreaterThan(0)
            .WithMessage("'DomainId' must be a positive sending domain ID.");

        RuleFor(t => t.Status)
            .Must(ContactStatus.IsDefined)
            .WithMessage("'Status' must be either subscribed or unsubscribed.");
    }
}
