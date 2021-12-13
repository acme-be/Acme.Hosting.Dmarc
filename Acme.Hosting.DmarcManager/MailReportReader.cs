// <copyright file="MailReportReader.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.DmarcManager;

using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Threading.Tasks;

using Acme.Hosting.Dmarc.Tools.Abstractions;
using Acme.Hosting.Dmarc.Tools.Abstractions.Models;
using Acme.Hosting.Dmarc.Tools.Options;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

public class MailReportReader
{
    private readonly IXmlReportStorage xmlReportStorage;

    public MailReportReader(IXmlReportStorage xmlReportStorage)
    {
        this.xmlReportStorage = xmlReportStorage;
    }

    [FunctionName("MailReportReader")]
    public async Task RunAsync([ServiceBusTrigger(ServiceBusOptions.Queues.RawReports, Connection = "ServiceBusConnectionString")] string myQueueItem, ILogger log)
    {
        log.LogInformation("ServiceBus queue trigger function processed message: {myQueueItem}", myQueueItem);

        var rawReport = JsonSerializer.Deserialize<RawReport>(myQueueItem);

        if (rawReport == null)
        {
            throw new ArgumentNullException(nameof(myQueueItem), "The item or the parsed result is null");
        }

        switch (rawReport.ContentType)
        {
            case "application/zip":
                await this.ExecuteZipAsync(rawReport);
                break;
            case "application/gzip":
                await this.ExecuteGZipAsync(rawReport);
                break;
            default:
                throw new NotImplementedException($"The parser for type \"{rawReport.ContentType}\" is not implemented.");
        }
    }

    private async Task ExecuteGZipAsync(RawReport rawReport)
    {
        await using var compressedFileStream = new MemoryStream(rawReport.Content);
        await using var gZipStream = new GZipStream(compressedFileStream, CompressionMode.Decompress);
        using var reader = new StreamReader(gZipStream);
        var xmlContent = await reader.ReadToEndAsync();
        await this.xmlReportStorage.StoreAsync(new XmlReport(xmlContent));
    }

    private async Task ExecuteZipAsync(RawReport rawReport)
    {
        await using var compressedFileStream = new MemoryStream(rawReport.Content);
        using var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Read, false);

        foreach (var archiveEntry in zipArchive.Entries)
        {
            await using var archiveStream = archiveEntry.Open();
            using var reader = new StreamReader(archiveStream);
            var xmlContent = await reader.ReadToEndAsync();
            await this.xmlReportStorage.StoreAsync(new XmlReport(xmlContent));
        }
    }
}