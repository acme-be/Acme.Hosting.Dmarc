// <copyright file="XmlReportParser.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Acme.Hosting.DmarcManager;

using System.Text.Json;

using Acme.Hosting.Dmarc.Tools.Abstractions;
using Acme.Hosting.Dmarc.Tools.Abstractions.Models;
using Acme.Hosting.Dmarc.Tools.Options;

public class XmlReportParser
{
    private readonly IXmlReportGenerator xmlReportGenerator;

    public XmlReportParser(IXmlReportGenerator xmlReportGenerator)
    {
        this.xmlReportGenerator = xmlReportGenerator;
    }

    [FunctionName("XmlReportParser")]
    public async Task RunAsync([ServiceBusTrigger(ServiceBusOptions.Queues.Reports, Connection = "ServiceBusConnectionString")] string myQueueItem, ILogger log)
    {
        log.LogInformation("ServiceBus queue trigger function processed message: {myQueueItem}", myQueueItem);

        var xmlReport = JsonSerializer.Deserialize<XmlReport>(myQueueItem);

        if (xmlReport == null)
        {
            throw new ArgumentNullException(nameof(myQueueItem), "The item or the parsed result is null");
        }

        await this.xmlReportGenerator.GenerateAsync(xmlReport);
    }
}