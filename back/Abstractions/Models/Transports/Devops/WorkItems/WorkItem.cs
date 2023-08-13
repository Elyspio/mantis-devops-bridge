using MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.Payloads;

namespace MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems;

public sealed class WorkItem : CreateWorkItemPayload
{
	public required int Id { get; init; }

	public required DateTime CreatedAt { get; set; }
	public required DateTime UpdatedAt { get; init; }
}