namespace MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems;

public class WorkItem : CreateWorkItemPayload
{
	public required int Id { get; set; }

	public required DateTime CreatedAt { get; set; }
	public required DateTime UpdatedAt { get; set; }
}