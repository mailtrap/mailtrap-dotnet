namespace Mailtrap.TestingMessages.Models;


/// <summary>
/// Represents blacklist report for email message.
/// </summary>
public sealed record BlacklistReport
{
    /// <summary>
    /// Gets overall blacklist status of the message.
    /// </summary>
    ///
    /// <value>
    /// Overall blacklist status of the message.
    /// </value>
    [JsonPropertyName("result")]
    [JsonPropertyOrder(1)]
    public BlacklistReportResult Result { get; set; } = BlacklistReportResult.Unknown;

    /// <summary>
    /// Gets sending domain of the message.
    /// </summary>
    ///
    /// <value>
    /// Sending domain of the message.
    /// </value>
    [JsonPropertyName("domain")]
    [JsonPropertyOrder(2)]
    public string? Domain { get; set; }

    /// <summary>
    /// Gets IP of the SMTP server.
    /// </summary>
    ///
    /// <value>
    /// IP of the SMTP server.
    /// </value>
    [JsonPropertyName("ip")]
    [JsonPropertyOrder(3)]
    public string? Ip { get; set; }

    /// <summary>
    /// Gets a collection of blacklist report details from different reporting sources.
    /// </summary>
    ///
    /// <value>
    /// A collection of blacklist report details from different reporting sources.
    /// </value>
    [JsonPropertyName("report")]
    [JsonPropertyOrder(4)]
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<BlacklistReportItem> Report { get; } = [];
}
