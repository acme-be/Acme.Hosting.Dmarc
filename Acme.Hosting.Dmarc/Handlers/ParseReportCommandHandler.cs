// <copyright file="ParseReportCommandHandler.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

using System.Text;
using Acme.Hosting.Dmarc.Commands;
using DmarcRua;
using MediatR;

namespace Acme.Hosting.Dmarc.Handlers;

public class ParseReportCommandHandler: IRequestHandler<ParseReportCommand>
{
    public async Task<Unit> Handle(ParseReportCommand request, CancellationToken cancellationToken)
    {
        var aggregateReport = new AggregateReport();

        await using var stream = new MemoryStream(Encoding.Default.GetBytes(request.Xml));
        aggregateReport.ReadAggregateReport(stream);
        
        return Unit.Value;
    }
}