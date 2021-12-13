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

public static class MailReportAggregatorLauncher
{
    [FunctionName("MailReportAggregatorLauncher")]
    public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
    {
        log.LogInformation("MailReportAggregator executed by a http trigger function at: {date}", DateTime.UtcNow);

        var popConnectionString = Environment.GetEnvironmentVariable("PopConnectionString", EnvironmentVariableTarget.Process);

        if (string.IsNullOrWhiteSpace(popConnectionString))
        {
            log.LogError("Cannot execute the pop aggregation without a PopConnectionString.");
            return new BadRequestResult();
        }

        var popUserName = Environment.GetEnvironmentVariable("PopUserName", EnvironmentVariableTarget.Process);

        if (string.IsNullOrWhiteSpace(popUserName))
        {
            log.LogError("Cannot execute the pop aggregation without a PopUserName.");
            return new BadRequestResult();
        }

        var popPassword = Environment.GetEnvironmentVariable("PopPassword", EnvironmentVariableTarget.Process);

        if (string.IsNullOrWhiteSpace(popPassword))
        {
            log.LogError("Cannot execute the pop aggregation without a PopPassword.");
            return new BadRequestResult();
        }

        var pop3Aggregator = new Pop3Aggregator(log, popConnectionString, popUserName, popPassword);
        await pop3Aggregator.ExecuteAsync();

        return new EmptyResult();
    }
}