// <copyright file="RawReportStorage.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Tools.Stores;

using Acme.Hosting.Dmarc.Tools.Abstractions;
using Acme.Hosting.Dmarc.Tools.Abstractions.Models;
using Acme.Hosting.Dmarc.Tools.Options;

using Azure.Messaging.ServiceBus;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class RawReportStorage: IRawReportStorage
{
    private readonly ILogger<RawReportStorage> logger;
    private readonly ServiceBusOptions serviceBusOptions;

    public RawReportStorage(ILogger<RawReportStorage> logger, IOptions<ServiceBusOptions> serviceBusOptions)
    {
        this.logger = logger;
        this.serviceBusOptions = serviceBusOptions.Value;
    }

    public async Task StoreAsync(RawReport report)
    {
        this.logger.LogInformation("Sending raw report {fileName} to the service bus", report.FileName);

        await using var client = new ServiceBusClient(this.serviceBusOptions.ConnectionString);
        var sender = client.CreateSender(ServiceBusOptions.Queues.RawReports);

        var message = new ServiceBusMessage();
        message.ContentType = "application/json";
        message.Body = new BinaryData(report);
        await sender.SendMessageAsync(message);
    }
}