namespace Mailtrap.Emails.Requests;


/// <summary>
/// A set of helper methods to streamline <see cref="EmailRequest"/> instance construction using fluent style.
/// </summary>
public static class EmailRequestBuilder
{
    #region From

    /// <summary>
    /// Sets provided <paramref name="sender"/> to the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <see cref="EmailRequest"/> instance to update.
    /// </param>
    ///
    /// <param name="sender">
    /// <see cref="EmailAddress"/> object to initialize request's <see cref="EmailRequest.From"/> property.
    /// </param>
    ///
    /// <returns>
    /// Updated <see cref="EmailRequest"/> instance so subsequent calls can be chained.
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> or <paramref name="sender"/> is <see langword="null"/>.
    /// </exception>
    ///
    /// <remarks>
    /// Subsequent calls will override value that was set before (last wins).
    /// </remarks>
    public static T From<T>(this T request, EmailAddress sender) where T : EmailRequest
    {
        Ensure.NotNull(request, nameof(request));
        Ensure.NotNull(sender, nameof(sender));

        request.From = sender;

        return request;
    }

    /// <summary>
    /// Sets provided <paramref name="email"/> and <paramref name="displayName"/> as sender
    /// for the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/param[@name='request']"/>
    /// </param>
    ///
    /// <param name="email">
    /// <para>
    /// Sender's email address.
    /// </para>
    /// <para>
    /// Required. Must be valid email address.
    /// </para>
    /// </param>
    ///
    /// <param name="displayName">
    /// Optional sender's display name.
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/returns"/>
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> is <see langword="null"/>.<br />
    /// When <paramref name="email"/> is <see langword="null"/> or <see cref="string.Empty"/>.
    /// </exception>
    ///
    /// <remarks>
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/remarks"/>
    /// </remarks>
    public static T From<T>(this T request, string email, string? displayName = default) where T : EmailRequest
    {
        Ensure.NotNull(request, nameof(request));

        return request.From(new EmailAddress(email, displayName));
    }

    #endregion



    #region Reply To

    /// <summary>
    /// Sets provided <paramref name="replyTo"/> address in the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <see cref="EmailRequest"/> instance to update.
    /// </param>
    ///
    /// <param name="replyTo">
    /// <see cref="EmailAddress"/> object to initialize request's <see cref="EmailRequest.ReplyTo"/> property.
    /// </param>
    ///
    /// <returns>
    /// Updated <see cref="EmailRequest"/> instance so subsequent calls can be chained.
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> is <see langword="null"/>.
    /// </exception>
    ///
    /// <remarks>
    /// Subsequent calls will override value that was set before (last wins).
    /// </remarks>
    public static T ReplyTo<T>(this T request, EmailAddress? replyTo) where T : EmailRequest
    {
        Ensure.NotNull(request, nameof(request));

        request.ReplyTo = replyTo;

        return request;
    }

    /// <summary>
    /// Sets provided <paramref name="email"/> and <paramref name="displayName"/> as 'Reply To' address
    /// in the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="ReplyTo{T}(T, EmailAddress?)" path="/param[@name='request']"/>
    /// </param>
    ///
    /// <param name="email">
    /// <para>
    /// 'Reply To' email address.
    /// </para>
    /// <para>
    /// Required. Must be valid email address.
    /// </para>
    /// </param>
    ///
    /// <param name="displayName">
    /// Optional 'Reply To' display name.
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="ReplyTo{T}(T, EmailAddress?)" path="/returns"/>
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> is <see langword="null"/>.<br />
    /// When <paramref name="email"/> is <see langword="null"/> or <see cref="string.Empty"/>.
    /// </exception>
    ///
    /// <remarks>
    /// <inheritdoc cref="ReplyTo{T}(T, EmailAddress?)" path="/remarks"/>
    /// </remarks>
    public static T ReplyTo<T>(this T request, string email, string? displayName = default) where T : EmailRequest
    {
        Ensure.NotNull(request, nameof(request));

        return request.ReplyTo(new EmailAddress(email, displayName));
    }

    #endregion



    #region Attachments

    /// <summary>
    /// Adds provided <paramref name="attachments"/> to the <see cref="EmailRequest.Attachments"/>
    /// collection of the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/param[@name='request']"/>
    /// </param>
    ///
    /// <param name="attachments">
    /// One or more <see cref="Attachment"/> objects to add to the request's
    /// <see cref="EmailRequest.Attachments"/> collection.
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/returns"/>
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> or <paramref name="attachments"/> is <see langword="null"/>.
    /// </exception>
    ///
    /// <remarks>
    /// Duplicates can be added by calling this method multiple times with the same <see cref="Attachment"/> object.
    /// </remarks>
    public static T Attach<T>(this T request, params Attachment[] attachments) where T : EmailRequest
    {
        Ensure.NotNull(request, nameof(request));
        Ensure.NotNull(request.Attachments, nameof(request.Attachments));
        Ensure.NotNull(attachments, nameof(attachments));

        request.Attachments.AddRange(attachments);

        return request;
    }

    /// <summary>
    /// Adds provided attachment to the <see cref="EmailRequest.Attachments"/>
    /// collection of the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="Attach{T}(T, Attachment[])" path="/param[@name='request']"/>
    /// </param>
    ///
    /// <param name="content">
    /// <inheritdoc cref="Attachment(string, string, DispositionType?, string?, string?)" path="/param[@name='content']/*"/>
    /// </param>
    ///
    /// <param name="fileName">
    /// <inheritdoc cref="Attachment(string, string, DispositionType?, string?, string?)" path="/param[@name='fileName']/*"/>
    /// </param>
    ///
    /// <param name="disposition">
    /// <inheritdoc cref="Attachment(string, string, DispositionType?, string?, string?)" path="/param[@name='disposition']/*"/>
    /// </param>
    ///
    /// <param name="mimeType">
    /// <inheritdoc cref="Attachment(string, string, DispositionType?, string?, string?)" path="/param[@name='mimeType']/*"/>
    /// </param>
    ///
    /// <param name="contentId">
    /// <inheritdoc cref="Attachment(string, string, DispositionType?, string?, string?)" path="/param[@name='contentId']/*"/>
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="Attach{T}(T, Attachment[])" path="/returns"/>
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> is <see langword="null"/>.<br />
    /// When <paramref name="content"/> is <see langword="null"/> or <see cref="string.Empty"/>.<br />
    /// When <paramref name="fileName"/> is <see langword="null"/> or <see cref="string.Empty"/>.
    /// </exception>
    ///
    /// <remarks>
    /// Duplicates can be added by calling this method multiple times with the same parameters.
    /// </remarks>
    public static T Attach<T>(this T request,
        string content,
        string fileName,
        DispositionType? disposition = default,
        string? mimeType = default,
        string? contentId = default)
        where T : EmailRequest
    {
        Ensure.NotNull(request, nameof(request));

        return request.Attach(new Attachment(content, fileName, disposition, mimeType, contentId));
    }

    #endregion



    #region Headers

    /// <summary>
    /// Adds provided <paramref name="headers"/> to the <see cref="EmailRequest.Headers"/>
    /// collection of the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/param[@name='request']"/>
    /// </param>
    ///
    /// <param name="headers">
    /// One or more key/value pairs to add to the request's <see cref="EmailRequest.Headers"/> collection.
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/returns"/>
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> or <paramref name="headers"/> is <see langword="null"/>.
    /// </exception>
    ///
    /// <remarks>
    /// Any existing headers with the same keys will be overridden.
    /// </remarks>
    public static T Header<T>(this T request, params KeyValuePair<string, string>[] headers) where T : EmailRequest
    {
        Ensure.NotNull(request, nameof(request));
        Ensure.NotNull(headers, nameof(headers));

        foreach (var header in headers)
        {
            request.Header(header.Key, header.Value);
        }

        return request;
    }

    /// <summary>
    /// Adds provided header to the <see cref="EmailRequest.Headers"/>
    /// collection of the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/param[@name='request']"/>
    /// </param>
    ///
    /// <param name="key">
    /// Header key to add.
    /// </param>
    ///
    /// <param name="value">
    /// Header value to add.
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/returns"/>
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> is <see langword="null"/>.<br />
    /// When <paramref name="key"/> is <see langword="null"/> or <see cref="string.Empty"/>.
    /// </exception>
    ///
    /// <remarks>
    /// Any existing header with the same key will be overridden.
    /// </remarks>
    public static T Header<T>(this T request, string key, string value) where T : EmailRequest
    {
        Ensure.NotNull(request, nameof(request));
        Ensure.NotNull(request.Headers, nameof(request.Headers));
        Ensure.NotNullOrEmpty(key, nameof(key));

        request.Headers[key] = value;

        return request;
    }

    #endregion



    #region Custom Variables

    /// <summary>
    /// Adds provided <paramref name="variables"/> to the <see cref="EmailRequest.CustomVariables"/>
    /// collection of the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/param[@name='request']"/>
    /// </param>
    ///
    /// <param name="variables">
    /// One or more key/value pairs to add to the request's <see cref="EmailRequest.CustomVariables"/> collection.
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/returns"/>
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> or <paramref name="variables"/> is <see langword="null"/>.
    /// </exception>
    ///
    /// <remarks>
    /// Any existing variables with the same keys will be overridden.
    /// </remarks>
    public static T CustomVariable<T>(this T request, params KeyValuePair<string, string>[] variables) where T : EmailRequest
    {
        Ensure.NotNull(request, nameof(request));
        Ensure.NotNull(variables, nameof(variables));

        foreach (var variable in variables)
        {
            request.CustomVariable(variable.Key, variable.Value);
        }

        return request;
    }

    /// <summary>
    /// Adds provided custom variable to the <see cref="EmailRequest.CustomVariables"/>
    /// collection of the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/param[@name='request']"/>
    /// </param>
    ///
    /// <param name="key">
    /// Variable key to add.
    /// </param>
    ///
    /// <param name="value">
    /// Variable value to add.
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/returns"/>
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> is <see langword="null"/>.<br />
    /// When <paramref name="key"/> is <see langword="null"/> or <see cref="string.Empty"/>.
    /// </exception>
    ///
    /// <remarks>
    /// Any existing variable with the same key will be overridden.
    /// </remarks>
    public static T CustomVariable<T>(this T request, string key, string value) where T : EmailRequest
    {
        Ensure.NotNull(request, nameof(request));
        Ensure.NotNull(request.CustomVariables, nameof(request.CustomVariables));
        Ensure.NotNullOrEmpty(key, nameof(key));

        request.CustomVariables[key] = value;

        return request;
    }

    #endregion

    #region Subject

    /// <summary>
    /// Sets provided <paramref name="subject"/> to the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/param[@name='request']"/>
    /// </param>
    ///
    /// <param name="subject">
    /// Value to initialize request's <see cref="EmailRequest.Subject"/> property.
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/returns"/>
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> is <see langword="null"/>.<br />
    /// When <paramref name="subject"/> is <see langword="null"/> or <see cref="string.Empty"/>.
    /// </exception>
    ///
    /// <remarks>
    /// Subsequent calls will override value that was set before (last wins).
    /// <para>
    /// Value must remain <see langword="null"/> if <see cref="EmailRequest.TemplateId"/> is used
    /// to create email from template.
    /// </para>
    /// </remarks>
    public static T Subject<T>(this T request, string subject) where T : EmailRequest
    {
        Ensure.NotNull(request, nameof(request));
        Ensure.NotNullOrEmpty(subject, nameof(subject));

        request.Subject = subject;

        return request;
    }

    #endregion

    #region Text

    /// <summary>
    /// Sets provided <paramref name="text"/> to the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/param[@name='request']"/>
    /// </param>
    ///
    /// <param name="text">
    /// Value to initialize request's <see cref="EmailRequest.TextBody"/> property.
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/returns"/>
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> is <see langword="null"/>.
    /// </exception>
    ///
    /// <remarks>
    /// <para>
    /// Subsequent calls will override value that was set before (last wins).
    /// </para>
    /// <para>
    /// Value must remain <see langword="null"/> if <see cref="EmailRequest.TemplateId"/> is used
    /// to create email from template.
    /// </para>
    /// </remarks>
    public static T Text<T>(this T request, string? text) where T : EmailRequest
    {
        Ensure.NotNull(request, nameof(request));

        request.TextBody = text;

        return request;
    }

    #endregion

    #region Html

    /// <summary>
    /// Sets provided <paramref name="html"/> to the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/param[@name='request']"/>
    /// </param>
    ///
    /// <param name="html">
    /// Value to initialize request's <see cref="EmailRequest.HtmlBody"/> property.
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/returns"/>
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> is <see langword="null"/>.
    /// </exception>
    ///
    /// <remarks>
    /// <para>
    /// It is a caller responsibility to ensure that <paramref name="html"/> contains a valid,
    /// well-formed HTML markup and is sanitized properly.
    /// </para>
    /// <para>
    /// Subsequent calls will override value that was set before (last wins).
    /// </para>
    /// <para>
    /// Value must remain <see langword="null"/> if <see cref="EmailRequest.TemplateId"/> is used
    /// to create email from template.
    /// </para>
    /// </remarks>
    public static T Html<T>(this T request, string? html) where T : EmailRequest
    {
        Ensure.NotNull(request, nameof(request));

        request.HtmlBody = html;

        return request;
    }

    #endregion

    #region Category

    /// <summary>
    /// Sets provided <paramref name="category"/> to the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/param[@name='request']"/>
    /// </param>
    ///
    /// <param name="category">
    /// Value to initialize request's <see cref="EmailRequest.Category"/> property.
    /// <para>
    /// Should be less or equal to 255 characters.
    /// </para>
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/returns"/>
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> is <see langword="null"/>.
    /// </exception>
    ///
    /// <remarks>
    /// Subsequent calls will override value that was set before (last wins).
    /// <para>
    /// Value must remain <see langword="null"/> if <see cref="EmailRequest.TemplateId"/> is used
    /// to create email from template.
    /// </para>
    /// </remarks>
    public static T Category<T>(this T request, string? category) where T : EmailRequest
    {
        Ensure.NotNull(request, nameof(request));

        request.Category = category;

        return request;
    }

    #endregion

    #region Template

    /// <summary>
    /// Sets provided <paramref name="templateId"/> to the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/param[@name='request']"/>
    /// </param>
    ///
    /// <param name="templateId">
    /// Value containing UUID of the template to initialize request's <see cref="EmailRequest.TemplateId"/> property.
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/returns"/>
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> is <see langword="null"/>.<br />
    /// When <paramref name="templateId"/> is <see langword="null"/> or <see cref="string.Empty"/>.
    /// </exception>
    ///
    /// <remarks>
    /// Subsequent calls will override value that was set before (last wins).
    /// <para>
    /// When <see cref="EmailRequest.TemplateId"/> is set, then <see cref="EmailRequest.Subject"/>,
    /// <see cref="EmailRequest.TextBody"/>, <see cref="EmailRequest.HtmlBody"/>
    /// and <see cref="EmailRequest.Category"/> properties are forbidden and must be set to <see langword="null"/>.
    /// </para>
    /// </remarks>
    public static T Template<T>(this T request, string templateId) where T : EmailRequest
    {
        Ensure.NotNull(request, nameof(request));
        Ensure.NotNullOrEmpty(templateId, nameof(templateId));

        request.TemplateId = templateId;

        return request;
    }

    /// <summary>
    /// Sets provided <paramref name="templateVariables"/> to the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/param[@name='request']"/>
    /// </param>
    ///
    /// <param name="templateVariables">
    /// Value containing object to initialize request's <see cref="EmailRequest.TemplateVariables"/> property.
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="From{T}(T, EmailAddress)" path="/returns"/>
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> is <see langword="null"/>.
    /// </exception>
    ///
    /// <remarks>
    /// Subsequent calls will override value that was set before (last wins).
    /// <para>
    /// Will be used only in case template is set.
    /// </para>
    /// </remarks>
    public static T TemplateVariables<T>(this T request, object? templateVariables) where T : EmailRequest
    {
        Ensure.NotNull(request, nameof(request));

        request.TemplateVariables = templateVariables;

        return request;
    }

    #endregion
}
