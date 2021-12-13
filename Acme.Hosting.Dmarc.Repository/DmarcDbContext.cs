// <copyright file="DmarcDbContext.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Repository;

using Microsoft.EntityFrameworkCore;

public class DmarcDbContext : DbContext
{
    public DmarcDbContext(DbContextOptions<DmarcDbContext> options)
        : base(options)
    {
    }
}