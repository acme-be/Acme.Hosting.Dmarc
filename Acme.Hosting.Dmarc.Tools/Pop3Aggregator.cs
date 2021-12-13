// <copyright file="Pop3Aggregator.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Tools;

using MailKit.Net.Pop3;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class Pop3Aggregator : IPop3Aggregator
{
    private readonly ILogger<Pop3Aggregator> logger;
    private readonly Pop3Configuration options;

    public Pop3Aggregator(ILogger<Pop3Aggregator> logger, IOptions<Pop3Configuration> options)
    {
        this.logger = logger;
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

        this.logger.LogDebug("Desconnecting");
        await client.DisconnectAsync(true);
    }
}