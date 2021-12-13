// <copyright file="IXmlReportStorage.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Tools.Abstractions;

using Acme.Hosting.Dmarc.Tools.Abstractions.Models;

public interface IXmlReportStorage
{
    Task StoreAsync(XmlReport report);
}