// <copyright file="MailReportAggregator.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.DmarcManager;

using System;
using System.Threading.Tasks;

using Acme.Hosting.Dmarc.Tools.Abstractions;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

public class MailReportAggregator
{
    private readonly IPop3Aggregator pop3Aggregator;

    public MailReportAggregator(IPop3Aggregator pop3Aggregator)
    {
        this.pop3Aggregator = pop3Aggregator;
    }

    [FunctionName("MailReportAggregator")]
    public async Task RunAsync([TimerTrigger("0 0 * * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation("MailReportAggregator executed by a http trigger function at: {date}", DateTime.UtcNow);

        await this.pop3Aggregator.ExecuteAsync();
    }
}