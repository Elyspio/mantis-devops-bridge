using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;

namespace MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.Payloads;

public class CreateWorkItemPayload : Issue
{
	public required string Comments { get; init; }
}