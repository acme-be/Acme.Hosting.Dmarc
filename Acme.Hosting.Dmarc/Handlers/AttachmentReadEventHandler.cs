// <copyright file="AttachmentReadEventHandler.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Handlers;

using System.IO.Compression;

using Acme.Hosting.Dmarc.Events;

using MediatR;

using Microsoft.Extensions.Logging;

public class AttachmentReadEventHandler : INotificationHandler<AttachmentReadEvent>
{
    private readonly ILogger<AttachmentReadEventHandler> logger;
    private readonly IMediator mediator;

    public AttachmentReadEventHandler(ILogger<AttachmentReadEventHandler> logger, IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task Handle(AttachmentReadEvent @event, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Processing a new attachment with MimeType {MimeType}", @event.MimeType);

        switch (@event.MimeType)
        {
            case "application/zip":
                await this.ExecuteZipAsync(@event.Content);
                break;
            case "application/gzip":
                await this.ExecuteGZipAsync(@event.Content);
                break;
            default:
                throw new NotImplementedException($"The parser for type \"{@event.MimeType}\" is not implemented.");
        }
    }

    private async Task ExecuteGZipAsync(byte[] content)
    {
        await using var compressedFileStream = new MemoryStream(content);
        await using var gZipStream = new GZipStream(compressedFileStream, CompressionMode.Decompress);
        using var reader = new StreamReader(gZipStream);
        var xmlContent = await reader.ReadToEndAsync();
        await this.mediator.Publish(new XmlReportFoundEvent(xmlContent));
    }

    private async Task ExecuteZipAsync(byte[] content)
    {
        await using var compressedFileStream = new MemoryStream(content);
        using var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Read, false);

        foreach (var archiveEntry in zipArchive.Entries)
        {
            await using var archiveStream = archiveEntry.Open();
            using var reader = new StreamReader(archiveStream);
            var xmlContent = await reader.ReadToEndAsync();

            await this.mediator.Publish(new XmlReportFoundEvent(xmlContent));
        }
    }
}