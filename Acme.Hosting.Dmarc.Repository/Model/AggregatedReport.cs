// <copyright file="AggregatedReport.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Repository.Model;

public class AggregatedReport
{
    public Guid Id { get; set; }

    public string? OrgName { get; set; }

    public string? Email { get; set; }

    public string? ExtraContactInfo { get; set; }

    public string? ReportId { get; set; }
}