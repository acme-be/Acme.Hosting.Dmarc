// <copyright file="Pop3Aggregator.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Tools;

using MailKit.Net.Pop3;

using Microsoft.Extensions.Logging;

public class Pop3Aggregator
{
    private readonly ILogger logger;

    private readonly string popPassword;

    private readonly int popPort;

    private readonly string popServer;

    private readonly string popUserName;

    private readonly bool popUseSsl;

    public Pop3Aggregator(ILogger logger, string popConnectionString, string popUserName, string popPassword)
    {
        this.popUserName = popUserName;
        this.popPassword = popPassword;
        this.logger = logger;

        var parsedConnection = popConnectionString.Split(';');
        this.popServer = parsedConnection[0];
        this.popPort = int.Parse(parsedConnection[1]);
        this.popUseSsl = bool.Parse(parsedConnection[2]);
    }

    public async Task ExecuteAsync()
    {
        using var client = new Pop3Client();

        this.logger.LogDebug("Connecting to {popServer}:{popPort} ({popUseSsl})", this.popServer, this.popPort, this.popUseSsl);
        await client.ConnectAsync(this.popServer, this.popPort, this.popUseSsl);

        this.logger.LogDebug("Authenticating with {popUserName}", this.popUserName);
        await client.AuthenticateAsync(this.popUserName, this.popPassword);

        this.logger.LogDebug("Desconnecting");
        await client.DisconnectAsync(true);
    }
}