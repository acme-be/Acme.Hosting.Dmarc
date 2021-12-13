// <copyright file="Pop3Configuration.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Tools;

public class Pop3Configuration
{
    public string? Password { get; set; }

    public int Port { get; set; }

    public string? Server { get; set; }

    public string? UserName { get; set; }

    public bool UseSsl { get; set; }
}