// <copyright file="DmarcDbContext.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Repository;

using Acme.Hosting.Dmarc.Repository.Model;

using Microsoft.EntityFrameworkCore;

public class DmarcDbContext : DbContext
{
    public DmarcDbContext(DbContextOptions<DmarcDbContext> options)
        : base(options)
    {
    }

    public DbSet<AggregatedReport> AggregatedReports { get; set; } = null!;
}