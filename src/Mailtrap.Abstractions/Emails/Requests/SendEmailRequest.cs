namespace Mailtrap.Emails.Requests;


/// <summary>
/// Represents request object used to send email, including recipient lists.
/// </summary>
public sealed record SendEmailRequest : EmailRequest
{
    /// <summary>
    /// Gets or sets a collection of <see cref="EmailAddress"/> objects, defining who will receive a copy of email.
    /// </summary>
    ///
    /// <value>
    /// A collection of <see cref="EmailAddress"/> objects.
    /// </value>
    ///
    /// <remarks>
    /// Each object in this collection must contain the recipient's email address.<br />
    /// Each object in this collection may optionally contain the recipient's name.<br />
    /// At least one recipient must be specified in one of the collections: <see cref="To"/>, <see cref="Cc"/> or <see cref="Bcc"/>.<br />
    /// The total recipients across <see cref="To"/>, <see cref="Cc"/> and <see cref="Bcc"/> must not exceed 1000.
    /// </remarks>
    [JsonPropertyName("to")]
    [JsonPropertyOrder(210)]
    public IList<EmailAddress> To { get; set; } = [];

    /// <summary>
    /// Gets or sets a collection of <see cref="EmailAddress"/> objects, defining who will receive a carbon copy of email.
    /// </summary>
    ///
    /// <value>
    /// A collection of <see cref="EmailAddress"/> objects.
    /// </value>
    ///
    /// <remarks>
    /// Each object in this collection must contain the recipient's email address.<br />
    /// Each object in this collection may optionally contain the recipient's name.<br />
    /// At least one recipient must be specified in one of the collections: <see cref="To"/>, <see cref="Cc"/> or <see cref="Bcc"/>.<br />
    /// The total recipients across <see cref="To"/>, <see cref="Cc"/> and <see cref="Bcc"/> must not exceed 1000.
    /// </remarks>
    [JsonPropertyName("cc")]
    [JsonPropertyOrder(220)]
    public IList<EmailAddress> Cc { get; set; } = [];

    /// <summary>
    /// Gets or sets a collection of <see cref="EmailAddress"/> objects, defining who will receive a blind carbon copy of email.
    /// </summary>
    ///
    /// <value>
    /// A collection of <see cref="EmailAddress"/> objects.
    /// </value>
    ///
    /// <remarks>
    /// Each object in this collection must contain the recipient's email address.<br />
    /// Each object in this collection may optionally contain the recipient's name.<br />
    /// At least one recipient must be specified in one of the collections: <see cref="To"/>, <see cref="Cc"/> or <see cref="Bcc"/>.<br />
    /// The total recipients across <see cref="To"/>, <see cref="Cc"/> and <see cref="Bcc"/> must not exceed 1000.
    /// </remarks>
    [JsonPropertyName("bcc")]
    [JsonPropertyOrder(230)]
    public IList<EmailAddress> Bcc { get; set; } = [];


    /// <summary>
    /// Factory method that creates a new instance of <see cref="SendEmailRequest" /> request.
    /// </summary>
    ///
    /// <returns>
    /// New <see cref="SendEmailRequest"/> instance.
    /// </returns>
    public static new SendEmailRequest Create() => new();


    /// <inheritdoc />
    public override ValidationResult Validate()
    {
        return SendEmailRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
