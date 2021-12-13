// <copyright file="MailReportAggregator.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.DmarcManager;

using System;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

public static class MailReportAggregator
{
    [FunctionName("MailReportAggregator")]
    public static async Task RunAsync([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation("MailReportAggregator executed by a http trigger function at: {date}", DateTime.UtcNow);
    }
}