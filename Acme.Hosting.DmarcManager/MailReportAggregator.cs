// <copyright file="MailReportAggregator.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.DmarcManager;

using System;
using System.Threading.Tasks;

using Acme.Hosting.Dmarc.Events;

using MediatR;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

public class MailReportAggregator
{
    private readonly IMediator mediator;

    public MailReportAggregator(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [FunctionName("MailReportAggregator")]
    public async Task RunAsync([TimerTrigger("0 0 * * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation("MailReportAggregator executed by a http trigger function at: {date}", DateTime.UtcNow);

        await this.mediator.Publish(new FetchReportsEvent());
    }
}