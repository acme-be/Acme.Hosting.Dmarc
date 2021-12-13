// <copyright file="XmlReportGenerator.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Tools.Xml;

using System.Text;

using Acme.Hosting.Dmarc.Tools.Abstractions;
using Acme.Hosting.Dmarc.Tools.Abstractions.Models;

using DmarcRua;

public class XmlReportGenerator : IXmlReportGenerator
{
    public async Task GenerateAsync(XmlReport report)
    {
        var aggregateReport = new AggregateReport();

        await using var stream = new MemoryStream(Encoding.Default.GetBytes(report.Xml));
        aggregateReport.ReadAggregateReport(stream);
    }
}