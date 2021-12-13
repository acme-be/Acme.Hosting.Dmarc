// <copyright file="Startup.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

using Acme.Hosting.DmarcManager;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Acme.Hosting.DmarcManager;

using System;

using Acme.Hosting.Dmarc.Tools;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.Configure<Pop3Configuration>(configuration =>
        {
            var popConnectionString = GetEnvironmentVariable("PopConnectionString");

            var parsedConnection = popConnectionString.Split(';');
            configuration.Server = parsedConnection[0];
            configuration.Port = int.Parse(parsedConnection[1]);
            configuration.UseSsl = bool.Parse(parsedConnection[2]);

            configuration.UserName = GetEnvironmentVariable("PopUserName");
            configuration.Password = GetEnvironmentVariable("PopPassword");
        });

        builder.Services.AddSingleton<IPop3Aggregator, Pop3Aggregator>();
    }

    private static string GetEnvironmentVariable(string name)
    {
        var value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ApplicationException($"The configuration {value} must be specified in environment variables.");
        }

        return value;
    }
}