using System.Text;
using MantisDevopsBridge.Api.Abstractions.Common.Extensions;
using MantisDevopsBridge.Api.Abstractions.Common.Helpers;
using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Abstractions.Configs;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Clients;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Services;
using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.Payloads;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Payloads;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Tickets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MantisDevopsBridge.Api.Core.Services;

public sealed class IssueService(IMantisClient mantisClient, IDevopsBoardClient devopsBoardClient, ILogger<IssueService> logger, IOptionsMonitor<MantisConfig> mantisConfig)
	: TracingService(logger), IIssueService
{
	private const string CadenaClosedChar = "🔒";

	public async Task Synchronize()
	{
		using var _ = LogService();

		var allMantis = mantisClient.GetAllTickets();
		var boards = devopsBoardClient.GetWorkItems();

		await Task.WhenAll(allMantis, boards);

		var allItems = boards.Result.SelectMany(i => i.Value).ToList();


		#region Mantis -> Board

		var ticketToCreate = new List<Ticket>();
		var ticketToUpdate = new List<(int IdWorkItem, Ticket Ticket)>();

		foreach (var ticket in allMantis.Result)
		{
			var board = allItems.Find(i => i.IdMantis == ticket.IdMantis);
			if (board == default) ticketToCreate.Add(ticket);
			else if (board.Hash != ticket.Hash) ticketToUpdate.Add((board.Id, ticket));
		}

		if (ticketToCreate.Count != 0) await ticketToCreate.Parallelize((t, _) => CreateWorkItem(t));

		if (ticketToUpdate.Count != 0) await ticketToUpdate.Parallelize((pair, _) => UpdateWorkItem(pair.IdWorkItem, pair.Ticket));

		#endregion

		#region Delete Board without Mantis

		var itemsToDelete = allItems.Where(i => allMantis.Result.All(t => t.IdMantis != i.IdMantis)).ToList();

		if (itemsToDelete.Count != 0) await itemsToDelete.Parallelize((item, _) => devopsBoardClient.DeleteWorkItem(item.Id));

		#endregion

		#region Board -> Mantis

		var boardToUpdate = allItems
			.Except(itemsToDelete)
			.Where(b => allMantis.Result.FirstOrDefault(t => t.IdMantis == b.IdMantis)?.Dates.UpdatedAt < b.UpdatedAt)
			.ToList();

		if (boardToUpdate.Count != 0) await boardToUpdate.Parallelize((item, _) => UpdateTicket(item));

		#endregion
	}

	private async Task UpdateTicket(Issue item)
	{
		using var _ = LogService($"{Log.F(item.IdMantis)}");

		await mantisClient.UpdateTicket(new UpdateTicketPayload
		{
			IdMantis = item.IdMantis,
			Status = item.Status
		});
	}

	private async Task<WorkItem> CreateWorkItem(Ticket t)
	{
		using var _ = LogService($"{Log.F(t.IdMantis)}");

		return await devopsBoardClient.CreateWorkItem(new CreateWorkItemPayload
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
			MantisUpdatedAt = t.Dates.UpdatedAt ?? DateTime.Now,
			MantisCreatedAt = t.Dates.CreatedAt,
			Hash = t.Hash
		});
	}

	private async Task<WorkItem> UpdateWorkItem(int id, Ticket t)
	{
		using var _ = LogService();

		return await devopsBoardClient.UpdateWorkItem(new UpdateWorkItemPayload
		{
			Id = id,
			Comments = ComputeComments(t),
			Description = t.Description,
			Priority = t.Priority,
			Severity = t.Severity,
			Status = t.Status,
			Summary = t.Summary,
			MantisUpdatedAt = t.Dates.UpdatedAt ?? DateTime.Now,
			Hash = t.Hash
		});
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
			if (message.Private)
			{
				sb.Append($" {CadenaClosedChar}");
			}
			sb.Append($" {message.Text}</p>");
		}

		return $"<div>{sb}</div>";
	}
}