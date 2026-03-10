namespace Mailtrap.Stats.Models;


/// <summary>
/// Represents aggregated sending statistics.
/// </summary>
public sealed record SendingStats
{
    /// <summary>
    /// Gets the number of delivered emails.
    /// </summary>
    [JsonPropertyName("delivery_count")]
    [JsonPropertyOrder(1)]
    public int DeliveryCount { get; set; }

    /// <summary>
    /// Gets the delivery rate.
    /// </summary>
    [JsonPropertyName("delivery_rate")]
    [JsonPropertyOrder(2)]
    public double DeliveryRate { get; set; }

    /// <summary>
    /// Gets the number of bounced emails.
    /// </summary>
    [JsonPropertyName("bounce_count")]
    [JsonPropertyOrder(3)]
    public int BounceCount { get; set; }

    /// <summary>
    /// Gets the bounce rate.
    /// </summary>
    [JsonPropertyName("bounce_rate")]
    [JsonPropertyOrder(4)]
    public double BounceRate { get; set; }

    /// <summary>
    /// Gets the number of opened emails.
    /// </summary>
    [JsonPropertyName("open_count")]
    [JsonPropertyOrder(5)]
    public int OpenCount { get; set; }

    /// <summary>
    /// Gets the open rate.
    /// </summary>
    [JsonPropertyName("open_rate")]
    [JsonPropertyOrder(6)]
    public double OpenRate { get; set; }

    /// <summary>
    /// Gets the number of clicked emails.
    /// </summary>
    [JsonPropertyName("click_count")]
    [JsonPropertyOrder(7)]
    public int ClickCount { get; set; }

    /// <summary>
    /// Gets the click rate.
    /// </summary>
    [JsonPropertyName("click_rate")]
    [JsonPropertyOrder(8)]
    public double ClickRate { get; set; }

    /// <summary>
    /// Gets the number of spam reports.
    /// </summary>
    [JsonPropertyName("spam_count")]
    [JsonPropertyOrder(9)]
    public int SpamCount { get; set; }

    /// <summary>
    /// Gets the spam rate.
    /// </summary>
    [JsonPropertyName("spam_rate")]
    [JsonPropertyOrder(10)]
    public double SpamRate { get; set; }
}
