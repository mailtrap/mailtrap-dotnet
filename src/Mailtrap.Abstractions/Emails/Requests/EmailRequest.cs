namespace Mailtrap.Emails.Requests;


/// <summary>
/// Represents base request object used to send email.
/// </summary>
public record EmailRequest : IValidatable
{
    /// <summary>
    /// <para>
    /// Gets or sets <see cref="EmailAddress"/> instance representing email's sender.
    /// </para>
    /// <para>
    /// Required.
    /// </para>
    /// </summary>
    ///
    /// <value>
    /// Instance, representing email's sender address and name.
    /// </value>
    [JsonPropertyName("from")]
    [JsonPropertyOrder(10)]
    public EmailAddress? From { get; set; }

    /// <summary>
    /// Gets or sets <see cref="EmailAddress"/> representing 'Reply To' email field.
    /// </summary>
    ///
    /// <value>
    /// Representing 'Reply To' address and name.
    /// </value>
    [JsonPropertyName("reply_to")]
    [JsonPropertyOrder(20)]
    public EmailAddress? ReplyTo { get; set; }

    /// <summary>
    /// Gets or sets the global or 'message level' subject of email.<br />
    /// This may be overridden by subject lines set in personalizations.
    /// </summary>
    ///
    /// <value>
    /// Contains the subject of the email.
    /// </value>
    ///
    /// <remarks>
    /// Must be <see langword="null"/> if <see cref="TemplateId"/> is set.
    /// <para>
    /// Required in case <see cref="HtmlBody"/> and(or) <see cref="TextBody"/> is used.<br/>
    /// Should be non-empty string in this case.
    /// </para>
    /// </remarks>
    [JsonPropertyName("subject")]
    [JsonPropertyOrder(30)]
    public string? Subject { get; set; }

    /// <summary>
    /// Gets or sets the text version of the body of the email.
    /// </summary>
    ///
    /// <value>
    /// Contains the text body of the email.
    /// </value>
    ///
    /// <remarks>
    /// Must be <see langword="null"/> if <see cref="TemplateId"/> is set.<br />
    /// Otherwise, can be used along with <see cref="HtmlBody"/> to create a fall-back for non-html clients.<br />
    /// Required in the absence of <see cref="TemplateId"/> and <see cref="HtmlBody"/>.
    /// </remarks>
    [JsonPropertyName("text")]
    [JsonPropertyOrder(40)]
    public string? TextBody { get; set; }

    /// <summary>
    /// Gets or sets HTML version of the body of the email.
    /// </summary>
    ///
    /// <value>
    /// Contains the HTML body of the email.
    /// </value>
    ///
    /// <remarks>
    /// Must be <see langword="null"/> if <see cref="TemplateId"/> is set.<br />
    /// Required in the absence of <see cref="TemplateId"/> and <see cref="TextBody"/>.
    /// </remarks>
    [JsonPropertyName("html")]
    [JsonPropertyOrder(50)]
    public string? HtmlBody { get; set; }

    /// <summary>
    /// Gets a collection of <see cref="Attachment"/> objects, where you can specify any attachments you want to include.
    /// </summary>
    ///
    /// <value>
    /// A collection of <see cref="Attachment"/> objects.
    /// </value>
    [JsonPropertyName("attachments")]
    [JsonPropertyOrder(60)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<Attachment> Attachments { get; set; } = [];

    /// <summary>
    /// Gets a dictionary of header names and values to substitute for them.
    /// </summary>
    ///
    /// <value>
    /// A dictionary of header names and values.
    /// </value>
    ///
    /// <remarks>
    /// The key/value pairs must be strings.<br/>
    /// You must ensure these are properly encoded if they contain unicode characters.<br />
    /// These headers cannot be one of the reserved headers.<br />
    /// "Content-Transfer-Encoding" header will be ignored and replaced with "quoted-printable".
    /// </remarks>
    [JsonPropertyName("headers")]
    [JsonPropertyOrder(70)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Gets or sets the category of email.
    /// </summary>
    ///
    /// <value>
    /// Contains the category of the email.
    /// </value>
    ///
    /// <remarks>
    /// Should be less or equal to 255 characters.
    /// </remarks>
    [JsonPropertyName("category")]
    [JsonPropertyOrder(80)]
    public string? Category { get; set; }

    /// <summary>
    /// Gets a dictionary of values that are specific to the entire send
    /// that will be carried along with the email and its activity data.
    /// </summary>
    ///
    /// <value>
    /// A dictionary of variable keys and values.
    /// </value>
    ///
    /// <remarks>
    /// The key/value pairs must be strings.<br/>
    /// Total size of custom variables in JSON form must not exceed 1000 bytes.
    /// </remarks>
    [JsonPropertyName("custom_variables")]
    [JsonPropertyOrder(90)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IDictionary<string, string> CustomVariables { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Gets or sets UUID of email template.
    /// </summary>
    ///
    /// <value>
    /// Contains the UUID of email template.
    /// </value>
    ///
    /// <remarks>
    /// If provided, then <see cref="Subject"/>, <see cref="TextBody"/>  and <see cref="HtmlBody"/>
    /// properties are forbidden and must be <see langword="null"/>.<br />
    /// Email subject, text and html will be generated from template using optional <see cref="TemplateVariables"/>.
    /// </remarks>
    [JsonPropertyName("template_uuid")]
    [JsonPropertyOrder(100)]
    public string? TemplateId { get; set; }

    /// <summary>
    /// Gets or sets optional template variables that will be used to generate actual subject, text and html
    /// from email template.
    /// </summary>
    ///
    /// <value>
    /// Contains template variables object.
    /// </value>
    ///
    /// <remarks>
    /// Will be used only in case <see cref="TemplateId"/> is set.
    /// </remarks>
    [JsonPropertyName("template_variables")]
    [JsonPropertyOrder(110)]
    public object? TemplateVariables { get; set; }


    /// <summary>
    /// Factory method that creates a new instance of <see cref="EmailRequest" /> request.
    /// </summary>
    ///
    /// <returns>
    /// New <see cref="EmailRequest"/> instance.
    /// </returns>
    public static EmailRequest Create() => new();


    /// <inheritdoc />
    public virtual ValidationResult Validate()
    {
        return EmailRequestValidator.Instance
            .Validate(this)
            .ToMailtrapValidationResult();
    }
}
