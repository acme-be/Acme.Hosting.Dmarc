// <copyright file="IXmlReportParser.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Tools.Abstractions;

using Acme.Hosting.Dmarc.Tools.Abstractions.Models;

public interface IXmlReportGenerator
{
    Task GenerateAsync(XmlReport report);
}