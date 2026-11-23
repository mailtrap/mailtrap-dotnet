namespace Mailtrap.Contacts.Validators;


/// <summary>
/// Validator for <see cref="CreateContactRequest"/> and for <see cref="UpdateContactRequest"/> requests.<br />
/// Ensures contact's email is not empty and length is within the allowed range (2-100 characters).
/// </summary>
public sealed class ContactRequestValidator : AbstractValidator<ContactRequest>
{
    /// <summary>
    /// Static validator instance for reuse.
    /// </summary>
    public static ContactRequestValidator Instance { get; } = new();

    /// <summary>
    /// Primary constructor.
    /// </summary>
    public ContactRequestValidator()
    {
        RuleFor(r => r.Email).NotEmpty().Length(2, 100);
    }
}
