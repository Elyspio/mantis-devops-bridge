using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.Payloads;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems;

namespace MantisDevopsBridge.Api.Abstractions.Interfaces.Clients;

public interface IDevopsBoardClient
{
	Task<Dictionary<AppName, List<WorkItem>>> GetWorkItems();
	Task<WorkItem> CreateWorkItem(CreateWorkItemPayload workItem);
	Task<WorkItem> UpdateWorkItem(UpdateWorkItemPayload workItem);
	Task DeleteWorkItem(int id);
}