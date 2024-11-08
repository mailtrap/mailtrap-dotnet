﻿// -----------------------------------------------------------------------
// <copyright file="EmailAttachment.cs" company="Railsware Products Studio, LLC">
// Copyright (c) Railsware Products Studio, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------


namespace Mailtrap.Attachments.Models;


/// <summary>
/// Represents attachment metadata for email sent to sandbox.
/// </summary>
public sealed record EmailAttachment
{
    /// <summary>
    /// Gets attachment identifier.
    /// </summary>
    ///
    /// <value>
    /// Attachment identifier.
    /// </value>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(1)]
    [JsonRequired]
    public long Id { get; set; }

    /// <summary>
    /// Gets message identifier.
    /// </summary>
    ///
    /// <value>
    /// Message identifier.
    /// </value>
    [JsonPropertyName("message_id")]
    [JsonPropertyOrder(2)]
    [JsonRequired]
    public long MessageId { get; set; }

    /// <summary>
    /// Gets attachment file name.
    /// </summary>
    ///
    /// <value>
    /// Attachment file name.
    /// </value>
    [JsonPropertyName("filename")]
    [JsonPropertyOrder(3)]
    public string? FileName { get; set; }

    /// <summary>
    /// Gets disposition of the attachment.
    /// </summary>
    ///
    /// <value>
    /// Attachment disposition.
    /// </value>
    [JsonPropertyName("attachment_type")]
    [JsonPropertyOrder(4)]
    public DispositionType? AttachmentType { get; set; }

    /// <summary>
    /// Gets MIME type of the attachment's content.
    /// </summary>
    ///
    /// <value>
    /// MIME type of the attachment's content.
    /// </value>
    [JsonPropertyName("content_type")]
    [JsonPropertyOrder(5)]
    public string? ContentType { get; set; }

    /// <summary>
    /// Gets attachment content identifier.
    /// </summary>
    ///
    /// <value>
    /// Attachment content identifier.
    /// </value>
    [JsonPropertyName("content_id")]
    [JsonPropertyOrder(6)]
    public string? ContentId { get; set; }

    /// <summary>
    /// Gets transfer encoding of the attachment.
    /// </summary>
    ///
    /// <value>
    /// Transfer encoding of the attachment.
    /// </value>
    [JsonPropertyName("transfer_encoding")]
    [JsonPropertyOrder(7)]
    public string? TransferEncoding { get; set; }

    /// <summary>
    /// Get attachment size.
    /// </summary>
    ///
    /// <value>
    /// Attachment size.
    /// </value>
    [JsonPropertyName("attachment_size")]
    [JsonPropertyOrder(8)]
    public long? AttachmentSize { get; set; }

    /// <summary>
    /// Gets the timestamp when attachment was created.
    /// </summary>
    ///
    /// <value>
    /// Attachment creation timestamp.
    /// </value>
    [JsonPropertyName("created_at")]
    [JsonPropertyOrder(9)]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Gets the timestamp when attachment was updated.
    /// </summary>
    ///
    /// <value>
    /// Attachment last updated timestamp.
    /// </value>
    [JsonPropertyName("updated_at")]
    [JsonPropertyOrder(10)]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Gets attachment size in a human-readable format.
    /// </summary>
    ///
    /// <value>
    /// Attachment size in a human-readable format.<br />
    /// E.g. '456 Bytes'.
    /// </value>
    [JsonPropertyName("attachment_human_size")]
    [JsonPropertyOrder(11)]
    public string? AttachmentHumanSize { get; set; }

    /// <summary>
    /// Gets download path for the attachment.
    /// </summary>
    ///
    /// <value>
    /// Download path for the attachment.
    /// </value>
    [JsonPropertyName("download_path")]
    [JsonPropertyOrder(12)]
    public string? DownloadPath { get; set; }
}
