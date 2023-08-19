using MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.Payloads;

namespace MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems;

public sealed class WorkItem : CreateWorkItemPayload
{
	public required int Id { get; init; }
	public required DateTime CreatedAt { get; set; }
	public required DateTime UpdatedAt { get; init; }

	public override string ToString()
	{
		return $"{nameof(Id)}: {Id}, {nameof(CreatedAt)}: {CreatedAt}, {nameof(UpdatedAt)}: {UpdatedAt}, {base.ToString()}";
	}
}