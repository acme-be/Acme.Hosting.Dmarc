// <copyright file="ParseReportCommand.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

using MediatR;

namespace Acme.Hosting.Dmarc.Commands;

public record ParseReportCommand(string Xml): IRequest;