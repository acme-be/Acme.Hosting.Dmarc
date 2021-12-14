// <copyright file="FetchReports.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Events;

using MediatR;

public record FetchReportsEvent : INotification;