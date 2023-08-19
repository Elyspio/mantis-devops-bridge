using System.Text;
using MantisDevopsBridge.Api.Abstractions.Common.Extensions;
using MantisDevopsBridge.Api.Abstractions.Common.Helpers;
using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Abstractions.Configs;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Clients;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Repositories;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Services;
using MantisDevopsBridge.Api.Abstractions.Models.Entities;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.Payloads;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Payloads;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Tickets;
using MantisDevopsBridge.Api.Core.Exceptions;
using MantisDevopsBridge.Api.Core.Exceptions.WorkItems;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MantisDevopsBridge.Api.Core.Services;

public sealed class SynchroService(IMantisClient mantisClient, IDevopsBoardClient devopsBoardClient, ILogger<SynchroService> logger, IOptionsMonitor<MantisConfig> mantisConfig, ITicketStateRepository ticketStateRepository)
	: TracingService(logger), ISynchroService
{
	private const string CadenaClosedChar = "🔒";

	public async Task Synchronize()
	{
		using var logger = LogService();

		var allMantis = mantisClient.GetAllTickets();
		var boards = devopsBoardClient.GetWorkItems();
		var states = ticketStateRepository.GetAll();

		await Task.WhenAll(allMantis, boards, states);

		var allWorkItems = boards.Result;


		await UpdateBoard(states.Result, allMantis.Result, allWorkItems);

		var workItemsDeleted = await DeleteWorkItemsWithoutMantis(allWorkItems, allMantis.Result);

		await UpdateMantis(states.Result, allMantis.Result, allWorkItems, workItemsDeleted.Select(i => i.Id));
	}

	/// <summary>
	///     Mantis -> Devops
	/// </summary>
	/// <param name="states"></param>
	/// <param name="allMantis"></param>
	/// <param name="allWorkItems"></param>
	private async Task UpdateBoard(IEnumerable<TicketStateEntity> states, IEnumerable<Ticket> allMantis, IReadOnlyCollection<WorkItem> allWorkItems)
	{
		using var logger = LogService();

		var workItemsToCreate = new List<Ticket>();
		var workItemsToUpdate = new List<(int Id, Ticket Ticket, bool StateIsMissing)>();

		var statesByIdMantis = states.ToDictionary(s => s.IdMantis, s => s);

		foreach (var ticket in allMantis)
		{
			var wt = allWorkItems.FirstOrDefault(i => i.IdMantis == ticket.IdMantis);
			if (wt is null)
			{
				workItemsToCreate.Add(ticket);
				continue;
			}

			var stateFound = statesByIdMantis.TryGetValue(ticket.IdMantis, out var state);
			var ticketUpdatedAt = ticket.Dates.UpdatedAt ?? ticket.Dates.CreatedAt;
			if (!stateFound || (ticketUpdatedAt > wt.UpdatedAt && ticketUpdatedAt > state!.WorkItemSynchronizedAt && ticketUpdatedAt != state.MantisSynchronizedAt)) workItemsToUpdate.Add((wt.Id, ticket, !stateFound));
		}

		if (workItemsToCreate.Count != 0)
		{
			var createResult = await workItemsToCreate.Parallelize((t, _) => CreateWorkItem(t));
			if (createResult.Status == ParallelStatus.Faulted)
			{
				logger.Error($"Error while creating work items: {Log.Stringify(createResult.Exceptions)}");
			}
		}
		else logger.Debug("No work item to create");

		if (workItemsToUpdate.Count != 0)
		{
			var updateResult = await workItemsToUpdate.Parallelize((pair, _) => UpdateWorkItem(pair.Id, pair.Ticket, pair.StateIsMissing));
			if (updateResult.Status == ParallelStatus.Faulted)
			{
				logger.Error(new WorkItemUpdateException(updateResult.Exceptions.ToDictionary(pair => pair.Key.Ticket, pair => pair.Value)));
			}
		}
		else logger.Debug("No work item to update");
	}


	/// <summary>
	///     Devops -> Mantis
	/// </summary>
	/// <param name="states"></param>
	/// <param name="allMantisResult"></param>
	/// <param name="allWorkItems"></param>
	/// <param name="idWorkItemsDeleted"></param>
	private async Task UpdateMantis(IEnumerable<TicketStateEntity> states, IEnumerable<Ticket> allMantisResult, IEnumerable<WorkItem> allWorkItems, IEnumerable<int> idWorkItemsDeleted)
	{
		using var logger = LogService();

		var stateByIdWorkItem = states.ToDictionary(t => t.IdWorkItem, t => t);

		var mantisById = allMantisResult.ToDictionary(mantis => mantis.IdMantis, mantis => mantis);

		var workItemsToHandle = allWorkItems.ExceptBy(idWorkItemsDeleted, item => item.Id);

		var workItemsToSynchro = workItemsToHandle.Where(wt =>
		{
			var mantis = mantisById[wt.IdMantis];

			var stateFound = stateByIdWorkItem.TryGetValue(wt.Id, out var state);
			if (!stateFound) return false;

			return wt.UpdatedAt > mantis.Dates.UpdatedAtOrCreatedAt && wt.UpdatedAt > state!.MantisSynchronizedAt && wt.UpdatedAt != state.WorkItemSynchronizedAt;
		}).ToArray();

		if (workItemsToSynchro.Length != 0)
		{
			var deleteResult = await workItemsToSynchro.Parallelize((item, _) => UpdateTicket(item));
			if (deleteResult.Status == ParallelStatus.Faulted)
			{
				logger.Error(new WorkItemDeleteException(deleteResult.Exceptions));
			}
		}
		else logger.Debug("No mantis ticket to update");
	}


	/// <summary>
	///     Devops -> X
	/// </summary>
	/// <param name="allWorkItems"></param>
	/// <param name="allMantis"></param>
	/// <returns></returns>
	private async Task<WorkItem[]> DeleteWorkItemsWithoutMantis(IEnumerable<WorkItem> allWorkItems, IReadOnlyCollection<Ticket> allMantis)
	{
		using var logger = LogService();

		var itemsToDelete = allWorkItems.Where(i => allMantis.All(t => t.IdMantis != i.IdMantis)).ToArray();

		if (itemsToDelete.Length != 0)
			await itemsToDelete.Parallelize(async (item, _) =>
			{
				await devopsBoardClient.DeleteWorkItem(item.Id);
				await ticketStateRepository.DeleteByIdWorkItem(item.Id);
			});
		else
			logger.Debug("No work item to delete");

		return itemsToDelete;
	}

	private async Task UpdateTicket(WorkItem item)
	{
		using var _ = LogService($"{Log.F(item.IdMantis)}");

		await mantisClient.UpdateTicket(new UpdateTicketPayload
		{
			IdMantis = item.IdMantis,
			Status = item.Status,
			Users = item.Users
		});

		await ticketStateRepository.UpdateState(item.IdMantis);
	}

	private async Task CreateWorkItem(Ticket t)
	{
		using var _ = LogService($"{Log.F(t.IdMantis)}");

		var item = await devopsBoardClient.CreateWorkItem(new CreateWorkItemPayload
		{
			IdMantis = t.IdMantis,
			App = t.App,
			Comments = ComputeComments(t),
			Description = t.Description,
			StepsToReproduce = t.StepsToReproduce,
			Priority = t.Priority,
			Severity = t.Severity,
			Status = t.Status,
			Summary = t.Summary,
			Users = t.Users
		});

		await ticketStateRepository.Create(t.IdMantis, item.Id, t.Dates.UpdatedAtOrCreatedAt, item.UpdatedAt);
	}

	private async Task UpdateWorkItem(int id, Ticket t, bool stateIsMissing)
	{
		using var _ = LogService($"{Log.F(id)} {Log.F(t.IdMantis)} {Log.F(stateIsMissing)}");

		var wt = await devopsBoardClient.UpdateWorkItem(new UpdateWorkItemPayload
		{
			Id = id,
			Comments = ComputeComments(t),
			Description = t.Description,
			Priority = t.Priority,
			Severity = t.Severity,
			Status = t.Status,
			Summary = t.Summary,
			MantisUpdatedAt = t.Dates.UpdatedAt ?? DateTime.Now,
			Hash = t.Hash,
			Users = t.Users
		});

		if (stateIsMissing)
			await ticketStateRepository.Create(t.IdMantis, id, t.Dates.UpdatedAtOrCreatedAt, wt.UpdatedAt);
		else
			await ticketStateRepository.UpdateState(t.IdMantis);
	}

	private string ComputeComments(Ticket ticket)
	{
		var sb = new StringBuilder();

		sb.Append(
			$"""
			 <h1>Ticket Mantis n°
			 <a href="{mantisConfig.CurrentValue.Endpoint.Host}/view.php?id={ticket.IdMantis}">{ticket.IdMantis}</a>
			 </h1>
			 """
		);

		if (ticket.Messages.Count != 0) sb.Append(@"<h2>Messages</h2>");

		foreach (var message in ticket.Messages)
		{
			sb.Append($"<p>{message.CreatedAt:g} | {message.Reporter}:");
			if (message.Private) sb.Append($" {CadenaClosedChar}");

			sb.Append($" {message.Text}</p>");
		}

		return $"<div>{sb}</div>";
	}
}