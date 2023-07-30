namespace MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems;

public class WorkItem : CreateWorkItemPayload
{
	public required int Id { get; set; }
}