// <copyright file="IPop3Aggregator.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Tools.Abstractions;

public interface IPop3Aggregator
{
    Task ExecuteAsync();
}