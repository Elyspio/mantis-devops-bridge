using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;

namespace MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.Payloads;

public class UpdateWorkItemPayload
{
	public required int Id { get; init; }

	public required string Comments { get; init; }

	public required DateTime MantisUpdatedAt { get; init; }

	public required string Summary { get; init; }

	public required string Description { get; init; }

	public required TicketSeverity Severity { get; init; }

	public required TicketPriority Priority { get; init; }

	public required TicketStatus Status { get; init; }
	public required string Hash { get; init; }
}