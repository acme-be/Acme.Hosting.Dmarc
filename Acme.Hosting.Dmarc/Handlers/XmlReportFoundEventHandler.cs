// <copyright file="XmlReportFoundEventHandler.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

using Acme.Hosting.Dmarc.Commands;
using Acme.Hosting.Dmarc.Events;
using MediatR;

namespace Acme.Hosting.Dmarc.Handlers;

public class XmlReportFoundEventHandler: INotificationHandler<XmlReportFoundEvent>
{
    private readonly IMediator mediator;

    public XmlReportFoundEventHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task Handle(XmlReportFoundEvent notification, CancellationToken cancellationToken)
    {
        await this.mediator.Send(new ParseReportCommand(notification.Xml), cancellationToken);
    }
}