// <copyright file="Pop3Aggregator.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Tools.Mail;

using Acme.Hosting.Dmarc.Tools.Abstractions;
using Acme.Hosting.Dmarc.Tools.Abstractions.Models;
using Acme.Hosting.Dmarc.Tools.Options;

using MailKit.Net.Pop3;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class Pop3Aggregator : IPop3Aggregator
{
    private readonly ILogger<Pop3Aggregator> logger;
    private readonly Pop3Options options;
    private readonly IRawReportStorage rawReportStorage;

    public Pop3Aggregator(ILogger<Pop3Aggregator> logger, IOptions<Pop3Options> options, IRawReportStorage rawReportStorage)
    {
        this.logger = logger;
        this.rawReportStorage = rawReportStorage;
        this.options = options.Value;
    }

    public async Task ExecuteAsync()
    {
        using var client = new Pop3Client();

        this.logger.LogDebug("Connecting to {popServer}:{popPort} ({popUseSsl})", this.options.Server, this.options.Port, this.options.UseSsl);
        await client.ConnectAsync(this.options.Server, this.options.Port, this.options.UseSsl);

        this.logger.LogDebug("Authenticating with {popUserName}", this.options.UserName);
        await client.AuthenticateAsync(this.options.UserName, this.options.Password);

        this.logger.LogInformation("Client has {unread} unread messages", client.Count);

        for (var i = 0; i < client.Count; i++)
        {
            var message = await client.GetMessageAsync(i);

            foreach (var attachment in message.Attachments)
            {
                await using var memoryStream = new MemoryStream();
                await attachment.WriteToAsync(memoryStream);
                await memoryStream.FlushAsync();
                memoryStream.Seek(0, SeekOrigin.Begin);

                await this.rawReportStorage.StoreAsync(new RawReport(attachment.ContentDisposition.FileName, attachment.ContentType.MimeType, memoryStream.ToArray()));
            }

            await client.DeleteMessageAsync(i);
        }

        this.logger.LogDebug("Disconnecting");
        await client.DisconnectAsync(true);
    }
}