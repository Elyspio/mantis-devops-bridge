using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;

namespace MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems;

public class UpdateWorkItemPayload
{
	public required int Id { get; set; }

	public required string Comments { get; set; }

	public required DateTime MantisUpdatedAt { get; set; }

	public required string Summary { get; set; }

	public required string Description { get; set; }

	public required TicketSeverity Severity { get; set; }

	public required TicketPriority Priority { get; set; }

	public required TicketStatus Status { get; set; }
}