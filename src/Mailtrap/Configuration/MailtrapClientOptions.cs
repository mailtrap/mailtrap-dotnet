namespace Mailtrap.Configuration;


/// <summary>
/// A set of parameters to configure Mailtrap API client.
/// </summary>
public record MailtrapClientOptions
{
    /// <summary>
    /// <para>
    /// Gets default configuration.
    /// </para>
    /// </summary>
    ///
    /// <value>
    /// <para>
    /// Contains default configuration.
    /// </para>
    /// <para>
    /// Includes default values for send API, serialization and empty authentication settings.
    /// </para>
    /// </value>
    ///
    /// <remarks>
    /// Returns new object every time, thus it's safe to mutate returned value.
    /// </remarks>
    public static MailtrapClientOptions Default => new();

    /// <summary>
    /// <para>
    /// Gets or sets API authentication token.
    /// </para>
    /// <para>
    /// Required. Must be non-empty string.
    /// </para>
    /// </summary>
    /// 
    /// <value>
    /// Contains API authentication token.
    /// </value>
    public string ApiToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets of sets flag which controls JSON indentation for pretty or minified output.
    /// </summary>
    ///
    /// <value>
    /// <para>
    /// <see langword="false"/> when JSON should be minified.<br/>
    /// <see langword="true"/> when JSON should be indented.
    /// </para>
    /// <para>
    /// Default is <see langword="false"/>.
    /// </para>
    /// </value>
    public bool PrettyJson { get; set; } = false;

    /// <summary>
    /// Gets or sets flag which controls usage of Bulk API for outgoing emails.
    /// </summary>
    /// 
    /// <value>
    /// <para>
    /// When <see langword="false"/> emails will be sent as transactional.<br/>
    /// When <see langword="true"/> emails will be sent as bulk.
    /// </para>
    /// <para>
    /// Default is <see langword="false"/>.
    /// </para>
    /// </value>
    public bool UseBulkApi { get; set; } = false;

    /// <inheritdoc cref="SandboxId"/>
    ///
    /// <remarks>
    /// <para>
    /// Deprecated, use <see cref="SandboxId"/> instead. Reads and writes the same value.
    /// </para>
    /// <para>
    /// Declared before <see cref="SandboxId"/>, so that configuration binding sets <see cref="SandboxId"/> last:
    /// when a configuration section has both keys, <see cref="SandboxId"/> wins.
    /// </para>
    /// </remarks>
    [Obsolete("InboxId is deprecated and will be removed in a future major release; use SandboxId instead.")]
    public long? InboxId
    {
        get => SandboxId;
        set => SandboxId = value;
    }

    /// <summary>
    /// <para>
    /// Gets or sets Sandbox ID to route emails to.
    /// </para>
    /// <para>
    /// When set to non-default value, all emails will be sent to this sandbox.
    /// </para>
    /// </summary>
    ///
    /// <value>
    /// <para>
    /// Sandbox ID to use for sending emails.
    /// </para>
    /// <para>
    /// Default is <see langword="null"/>.
    /// </para>
    /// </value>
    public long? SandboxId { get; set; } = null;


    /// <summary>
    /// Primary instance constructor, which allows to set an authentication token.
    /// </summary>
    /// 
    /// <param name="apiToken">
    /// <para>
    /// API authentication token.
    /// </para>
    /// <para>
    /// Required. Must be non-empty string.
    /// </para>
    /// </param>
    /// 
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="apiToken"/> is <see langword="null"/> or <see cref="string.Empty"/>.
    /// </exception>
    public MailtrapClientOptions(string apiToken)
    {
        Ensure.NotNullOrEmpty(apiToken, nameof(apiToken));

        ApiToken = apiToken;
    }

    /// <summary>
    /// Parameterless instance constructor.
    /// </summary>
    public MailtrapClientOptions() { }



    /// <summary>
    /// Initializes current <see cref="MailtrapClientOptions"/> instance with values from <paramref name="source"/>.
    /// </summary>
    /// 
    /// <param name="source">
    /// Source <see cref="MailtrapClientOptions"/> instance to copy values from.
    /// </param>
    /// 
    /// <remarks>
    /// Performs a shallow copy.
    /// </remarks>
    public void Init(MailtrapClientOptions source)
    {
        Ensure.NotNull(source, nameof(source));

        ApiToken = source.ApiToken;
        PrettyJson = source.PrettyJson;
        UseBulkApi = source.UseBulkApi;
        SandboxId = source.SandboxId;
    }


    /// <summary>
    /// Converts <see cref="MailtrapClientOptions"/> instance to <see cref="JsonSerializerOptions"/>.
    /// </summary>
    internal JsonSerializerOptions ToJsonSerializerOptions()
    {
        return new JsonSerializerOptions(MailtrapJsonSerializerOptions.Default)
        {
            WriteIndented = PrettyJson
        };
    }
}
