﻿using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;
using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;

namespace MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Payloads;

public sealed class UpdateTicketPayload
{
	public required int IdMantis { get; init; }
	public required TicketStatus Status { get; init; }
	public required IssueUsers Users { get; set; }
}