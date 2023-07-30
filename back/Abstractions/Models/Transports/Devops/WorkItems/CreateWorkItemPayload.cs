using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;

namespace MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems;

public class CreateWorkItemPayload : Issue
{
	public required string Comments { get; set; }
}