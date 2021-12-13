// <copyright file="ServiceBusOptions.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Tools.Options;

public class ServiceBusOptions
{
    public string ConnectionString { get; set; } = string.Empty;

    public static class Queues
    {
        public const string RawReports = "RawReports";
        public const string Reports = "Reports";
    }
}