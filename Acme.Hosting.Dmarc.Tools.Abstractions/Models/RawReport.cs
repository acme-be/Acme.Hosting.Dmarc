// <copyright file="RawReport.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Tools.Abstractions.Models;

public record RawReport(string FileName, string ContentType, byte[] Content);