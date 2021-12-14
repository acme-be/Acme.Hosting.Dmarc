// <copyright file="ProcessAttachmentCommand.cs" company="Acme">
// Copyright (c) Acme. All rights reserved.
// </copyright>

namespace Acme.Hosting.Dmarc.Events;

using MediatR;

using MimeKit;

public record ProcessAttachmentEvent(MimeEntity Attachment) : INotification;