using MantisDevopsBridge.Api.Abstractions.Common.Extensions;
using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Clients;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Repositories;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace MantisDevopsBridge.Api.Core.Services;

public class WorkItemService(ILogger<WorkItemService> logger, IDevopsBoardClient devopsBoardClient, ITicketStateRepository stateRepository) : TracingService(logger), IWorkItemService
{
	public async Task DeleteAllWorkItems()
	{
		using var _ = LogService();

		var allWorkItems = await devopsBoardClient.GetWorkItems();

		await allWorkItems.Parallelize(async (wt, _) =>
		{
			await devopsBoardClient.DeleteWorkItem(wt.Id);
			await stateRepository.DeleteByIdWorkItem(wt.Id);
		});
	}
}