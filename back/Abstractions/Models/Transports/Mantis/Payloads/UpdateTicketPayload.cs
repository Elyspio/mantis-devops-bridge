using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;

namespace MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Payloads;

public class UpdateTicketPayload
{
	public required int IdMantis { get; init; }
	public required TicketStatus Status { get; init; }
	public required TicketPriority Priority { get; init; }
	public required TicketSeverity Severity { get; init; }
}