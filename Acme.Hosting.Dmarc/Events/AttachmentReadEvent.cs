// <copyright file="AttachmentReadEvent.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Events;

using MediatR;

public record AttachmentReadEvent(string FileName, string MimeType, byte[] Content) : INotification;