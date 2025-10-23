namespace Mailtrap.Emails.Requests;


/// <summary>
/// A set of helper methods to streamline <see cref="SendEmailRequest"/> instance construction using fluent style.
/// </summary>
public static class SendEmailRequestBuilder
{
    #region To

    /// <summary>
    /// Adds provided <paramref name="recipients"/> to the <see cref="SendEmailRequest.To"/>
    /// recipient collection of the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="EmailRequestBuilder.From{T}(T, EmailAddress)" path="/param[@name='request']"/>
    /// </param>
    /// 
    /// <param name="recipients">
    /// One or more <see cref="EmailAddress"/> objects to add to the request's recipient collection.
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="EmailRequestBuilder.From{T}(T, EmailAddress)" path="/returns"/>
    /// </returns>
    /// 
    /// <exception cref="ArgumentNullException" id="ArgumentNullException">
    /// When <paramref name="request"/> or <paramref name="recipients"/> is <see langword="null"/>.
    /// </exception>
    /// 
    /// <remarks>
    /// Duplicates can be added by calling this method multiple times with the same recipients.
    /// </remarks>
    public static SendEmailRequest To(this SendEmailRequest request, params EmailAddress[] recipients)
    {
        Ensure.NotNull(request, nameof(request));
        Ensure.NotNull(request.To, nameof(request.To));
        Ensure.NotNull(recipients, nameof(recipients));

        request.To.AddRange(recipients);

        return request;
    }

    /// <summary>
    /// Adds provided <paramref name="email"/> and <paramref name="displayName"/> tuple as recipient
    /// to the <see cref="SendEmailRequest.To"/> recipient collection of the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="To(SendEmailRequest, EmailAddress[])" path="/param[@name='request']"/>
    /// </param>
    /// 
    /// <param name="email">
    /// <para>
    /// Recipient's email address.
    /// </para>
    /// <para>
    /// Required. Must be valid email address.
    /// </para>
    /// </param>
    /// 
    /// <param name="displayName">
    /// Optional recipient's display name.
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="To(SendEmailRequest, EmailAddress[])" path="/returns"/>
    /// </returns>
    /// 
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> is <see langword="null"/>.<br />
    /// When <paramref name="email"/> is <see langword="null"/> or <see cref="string.Empty"/>.
    /// </exception>
    /// 
    /// <remarks>
    /// <inheritdoc cref="To(SendEmailRequest, EmailAddress[])" path="/remarks"/>
    /// </remarks>
    public static SendEmailRequest To(this SendEmailRequest request, string email, string? displayName = default)
    {
        Ensure.NotNull(request, nameof(request));

        return request.To(new EmailAddress(email, displayName));
    }

    #endregion



    #region Cc

    /// <summary>
    /// Adds provided <paramref name="recipients"/> to the <see cref="SendEmailRequest.Cc"/>
    /// recipient collection of the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="To(SendEmailRequest, EmailAddress[])" path="/param[@name='request']"/>
    /// </param>
    /// 
    /// <param name="recipients">
    /// <inheritdoc cref="To(SendEmailRequest, EmailAddress[])" path="/param[@name='recipients']"/>
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="To(SendEmailRequest, EmailAddress[])" path="/returns"/>
    /// </returns>
    /// 
    /// <exception cref="ArgumentNullException" id="ArgumentNullException">
    /// When <paramref name="request"/> or <paramref name="recipients"/> is <see langword="null"/>.
    /// </exception>
    /// 
    /// <remarks>
    /// <inheritdoc cref="To(SendEmailRequest, EmailAddress[])" path="/remarks"/>
    /// </remarks>
    public static SendEmailRequest Cc(this SendEmailRequest request, params EmailAddress[] recipients)
    {
        Ensure.NotNull(request, nameof(request));
        Ensure.NotNull(request.Cc, nameof(request.Cc));
        Ensure.NotNull(recipients, nameof(recipients));

        request.Cc.AddRange(recipients);

        return request;
    }

    /// <summary>
    /// Adds provided <paramref name="email"/> and <paramref name="displayName"/> tuple as recipient
    /// to the <see cref="SendEmailRequest.Cc"/> recipient collection of the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="To(SendEmailRequest, string, string?)" path="/param[@name='request']"/>
    /// </param>
    /// 
    /// <param name="email">
    /// <inheritdoc cref="To(SendEmailRequest, string, string?)" path="/param[@name='email']/*"/>
    /// </param>
    ///
    /// <param name="displayName">
    /// <inheritdoc cref="To(SendEmailRequest, string, string?)" path="/param[@name='displayName']"/>
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="To(SendEmailRequest, string, string?)" path="/returns"/>
    /// </returns>
    /// 
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> is <see langword="null"/>.<br />
    /// When <paramref name="email"/> is <see langword="null"/> or <see cref="string.Empty"/>.
    /// </exception>
    ///
    /// <remarks>
    /// <inheritdoc cref="To(SendEmailRequest, string, string?)" path="/remarks"/>
    /// </remarks>
    public static SendEmailRequest Cc(this SendEmailRequest request, string email, string? displayName = default)
    {
        Ensure.NotNull(request, nameof(request));

        return request.Cc(new EmailAddress(email, displayName));
    }

    #endregion



    #region Bcc

    /// <summary>
    /// Adds provided <paramref name="recipients"/> to the <see cref="SendEmailRequest.Bcc"/>
    /// recipient collection of the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="To(SendEmailRequest, EmailAddress[])" path="/param[@name='request']"/>
    /// </param>
    /// 
    /// <param name="recipients">
    /// <inheritdoc cref="To(SendEmailRequest, EmailAddress[])" path="/param[@name='recipients']"/>
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="To(SendEmailRequest, EmailAddress[])" path="/returns"/>
    /// </returns>
    ///
    /// <exception cref="ArgumentNullException" id="ArgumentNullException">
    /// When <paramref name="request"/> or <paramref name="recipients"/> is <see langword="null"/>.
    /// </exception>
    /// 
    /// <remarks>
    /// <inheritdoc cref="To(SendEmailRequest, EmailAddress[])" path="/remarks"/>
    /// </remarks>
    public static SendEmailRequest Bcc(this SendEmailRequest request, params EmailAddress[] recipients)
    {
        Ensure.NotNull(request, nameof(request));
        Ensure.NotNull(request.Bcc, nameof(request.Bcc));
        Ensure.NotNull(recipients, nameof(recipients));

        request.Bcc.AddRange(recipients);

        return request;
    }

    /// <summary>
    /// Adds provided <paramref name="email"/> and <paramref name="displayName"/> tuple as recipient
    /// to the <see cref="SendEmailRequest.Bcc"/> recipient collection of the <paramref name="request"/>.
    /// </summary>
    ///
    /// <param name="request">
    /// <inheritdoc cref="To(SendEmailRequest, string, string?)" path="/param[@name='request']"/>
    /// </param>
    /// 
    /// <param name="email">
    /// <inheritdoc cref="To(SendEmailRequest, string, string?)" path="/param[@name='email']/*"/>
    /// </param>
    ///
    /// <param name="displayName">
    /// <inheritdoc cref="To(SendEmailRequest, string, string?)" path="/param[@name='displayName']"/>
    /// </param>
    ///
    /// <returns>
    /// <inheritdoc cref="To(SendEmailRequest, string, string?)" path="/returns"/>
    /// </returns>
    /// 
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="request"/> is <see langword="null"/>.<br />
    /// When <paramref name="email"/> is <see langword="null"/> or <see cref="string.Empty"/>.
    /// </exception>
    ///
    /// <remarks>
    /// <inheritdoc cref="To(SendEmailRequest, string, string?)" path="/remarks"/>
    /// </remarks>
    public static SendEmailRequest Bcc(this SendEmailRequest request, string email, string? displayName = default)
    {
        Ensure.NotNull(request, nameof(request));

        return request.Bcc(new EmailAddress(email, displayName));
    }

    #endregion
}
