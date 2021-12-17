// <copyright file="FetchReportsHandler.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Handlers;

using Acme.Hosting.Dmarc.Events;
using Acme.Hosting.Dmarc.Options;

using MailKit.Net.Pop3;

using MediatR;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class FetchReportsEventHandler : INotificationHandler<FetchReportsEvent>
{
    private readonly ILogger<FetchReportsEventHandler> logger;
    private readonly IMediator mediator;
    private readonly Pop3Options options;

    public FetchReportsEventHandler(ILogger<FetchReportsEventHandler> logger, IOptions<Pop3Options> options, IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.options = options.Value;
    }

    public async Task Handle(FetchReportsEvent @event, CancellationToken cancellationToken)
    {
        using var client = new Pop3Client();

        this.logger.LogDebug("Connecting to {popServer}:{popPort} ({popUseSsl})", this.options.Server, this.options.Port, this.options.UseSsl);
        await client.ConnectAsync(this.options.Server, this.options.Port, this.options.UseSsl, cancellationToken);

        this.logger.LogDebug("Authenticating with {popUserName}", this.options.UserName);
        await client.AuthenticateAsync(this.options.UserName, this.options.Password, cancellationToken);

        this.logger.LogInformation("Client has {unread} unread messages", client.Count);

        for (var i = 0; i < client.Count; i++)
        {
            var message = await client.GetMessageAsync(i, cancellationToken);

            foreach (var attachment in message.Attachments)
            {
                await this.mediator.Publish(new ProcessAttachmentEvent(attachment), cancellationToken);
            }

            await client.DeleteMessageAsync(i, cancellationToken);
        }

        this.logger.LogDebug("Disconnecting");
        await client.DisconnectAsync(true, cancellationToken);
    }
}