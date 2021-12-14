// <copyright file="Pop3Options.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Tools.Options;

public class Pop3Options
{
    public string Password { get; set; } = string.Empty;

    public int Port { get; set; }

    public string Server { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public bool UseSsl { get; set; }
}