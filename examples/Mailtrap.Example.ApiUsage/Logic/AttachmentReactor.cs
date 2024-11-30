﻿// -----------------------------------------------------------------------
// <copyright file="AttachmentReactor.cs" company="Railsware Products Studio, LLC">
// Copyright (c) Railsware Products Studio, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------


namespace Mailtrap.Example.ApiUsage.Logic;


internal sealed class AttachmentReactor : Reactor
{
    public AttachmentReactor(IMailtrapClient mailtrapClient, ILogger<AttachmentReactor> logger)
        : base(mailtrapClient, logger) { }


    public async Task Process(long accountId, long inboxId, long messageId)
    {
        ITestingMessageResource messageResource = _mailtrapClient
            .Account(accountId)
            .Inbox(inboxId)
            .Message(messageId);

        // Get resource for attachments collection
        IAttachmentCollectionResource attachmentsResource = messageResource.Attachments();

        var attachmentFilter = new EmailAttachmentFilter
        {
            Disposition = DispositionType.Attachment
        };

        IList<EmailAttachment> attachments = await attachmentsResource.Fetch(attachmentFilter);

        EmailAttachment? attachment = attachments.FirstOrDefault();

        if (attachment is null)
        {
            _logger.LogWarning("No attachments found.");
            return;
        }

        // Get resource for specific attachment
        IAttachmentResource attachmentResource = messageResource.Attachment(attachment.Id);

        // Get attachment details
        attachment = await attachmentResource.GetDetails();
        _logger.LogInformation("Attachment: {Attachment}", attachment);
    }
}
