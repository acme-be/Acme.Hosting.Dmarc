// <copyright file="XmlReportGenerator.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Tools.Xml;

using System.Text;

using Acme.Hosting.Dmarc.Repository;
using Acme.Hosting.Dmarc.Repository.Model;
using Acme.Hosting.Dmarc.Tools.Abstractions;
using Acme.Hosting.Dmarc.Tools.Abstractions.Models;

using DmarcRua;

public class XmlReportGenerator : IXmlReportGenerator
{
    private readonly DmarcDbContext dbContext;

    public XmlReportGenerator(DmarcDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task GenerateAsync(XmlReport report)
    {
        var aggregateReport = new AggregateReport();

        await using var stream = new MemoryStream(Encoding.Default.GetBytes(report.Xml));
        aggregateReport.ReadAggregateReport(stream);

        var dbReport = new AggregatedReport
        {
            Id = Guid.NewGuid(),
            Email = aggregateReport.Feedback.ReportMetadata.Email,
            OrgName = aggregateReport.Feedback.ReportMetadata.OrgName,
            ReportId = aggregateReport.Feedback.ReportMetadata.ReportId,
            ExtraContactInfo = aggregateReport.Feedback.ReportMetadata.ExtraContactInfo,
        };

        this.dbContext.AggregatedReports.Add(dbReport);
        await this.dbContext.SaveChangesAsync();
    }
}