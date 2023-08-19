using MantisDevopsBridge.Api.Abstractions.Common.Extensions;
using MantisDevopsBridge.Api.Abstractions.Common.Helpers;
using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Clients;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Repositories;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace MantisDevopsBridge.Api.Core.Services;

public class TicketStateService(ILogger<TicketStateService> logger, ITicketStateRepository ticketStateRepository, IMantisClient mantisClient, IDevopsBoardClient boardClient) : TracingService(logger), ITicketStateService
{
	public async Task Regenerate()
	{
		using var logger = LogService();

		var mantisTask = mantisClient.GetAllTickets();
		var boardsTask = boardClient.GetWorkItems();
		var statesTask = ticketStateRepository.GetAll();

		await Task.WhenAll(mantisTask, boardsTask, statesTask);


		var tickets = mantisTask.Result;
		var workItems = boardsTask.Result;
		var states = statesTask.Result;


		var stateToCreate = tickets
			.ExceptBy(states.Select(s => s.IdMantis), t => t.IdMantis)
			.Where(t => workItems.Any(wt => wt.IdMantis == t.IdMantis))
			.Select(t => (Ticket: t, WorkItem: workItems.First(wt => wt.IdMantis == t.IdMantis)))
			.ToArray();

		logger.Debug($"States for the following tickets will be created: {Log.Stringify(stateToCreate.Select(s => s.Ticket.IdMantis))}");

		await stateToCreate.Parallelize((pair, _) => ticketStateRepository.Create(pair.Ticket.IdMantis, pair.WorkItem.Id, pair.Ticket.Dates.UpdatedAtOrCreatedAt, pair.WorkItem.UpdatedAt));
	}
}