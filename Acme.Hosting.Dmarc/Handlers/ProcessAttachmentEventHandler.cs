// <copyright file="ProcessAttachmentHandler.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Handlers;

using Acme.Hosting.Dmarc.Events;

using MediatR;

using Microsoft.Extensions.Logging;

using MimeKit;

public class ProcessAttachmentEventHandler: INotificationHandler<ProcessAttachmentEvent>
{
    private readonly ILogger<ProcessAttachmentEventHandler> logger;
    private readonly IMediator mediator;

    public ProcessAttachmentEventHandler(ILogger<ProcessAttachmentEventHandler> logger, IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task Handle(ProcessAttachmentEvent @event, CancellationToken cancellationToken)
    {
        await using var memoryStream = new MemoryStream();

        switch (@event.Attachment)
        {
            case MessagePart rfc822:
                this.logger.LogDebug("Found a attachment in rfc822 format.");
                await rfc822.Message.WriteToAsync(memoryStream, cancellationToken);
                break;
            case MimePart part:
                this.logger.LogDebug("Found a attachment in part format.");
                await part.Content.DecodeToAsync(memoryStream, cancellationToken);
                break;
        }

        await memoryStream.FlushAsync(cancellationToken);
        memoryStream.Seek(0, SeekOrigin.Begin);

        await this.mediator.Publish(new AttachmentReadEvent(@event.Attachment.ContentDisposition.FileName, @event.Attachment.ContentType.MimeType, memoryStream.ToArray()), cancellationToken);
    }
}