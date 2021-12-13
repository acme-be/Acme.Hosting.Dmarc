// <copyright file="MailReportAggregatorLauncher.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.DmarcManager;

using System;
using System.Threading.Tasks;

using Acme.Hosting.Dmarc.Tools;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

public class MailReportAggregatorLauncher
{
    private readonly IPop3Aggregator pop3Aggregator;

    public MailReportAggregatorLauncher(IPop3Aggregator pop3Aggregator)
    {
        this.pop3Aggregator = pop3Aggregator;
    }

    [FunctionName("MailReportAggregatorLauncher")]
    public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
    {
        log.LogInformation("MailReportAggregator executed by a http trigger function at: {date}", DateTime.UtcNow);

        await this.pop3Aggregator.ExecuteAsync();

        return new EmptyResult();
    }
}