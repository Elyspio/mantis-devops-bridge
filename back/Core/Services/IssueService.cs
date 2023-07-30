using System.Text;
using MantisDevopsBridge.Api.Abstractions.Common.Extensions;
using MantisDevopsBridge.Api.Abstractions.Common.Helpers;
using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Abstractions.Configs;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Clients;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Services;
using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Payloads;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Tickets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MantisDevopsBridge.Api.Core.Services;

public class IssueService(IMantisClient mantisClient, IDevopsBoardClient devopsBoardClient, ILogger<IssueService> logger, IOptionsMonitor<MantisConfig> mantisConfig)
	: TracingService(logger), IIssueService
{
	private const string CadenaClosedChar = "🔒";
	private const string CadenaOpenChar = "🔓";

	public async Task Synchronize()
	{
		using var _ = LogService();

		var tickets = mantisClient.GetAllTickets();
		var items = devopsBoardClient.GetWorkItems();

		await Task.WhenAll(tickets, items);

		var allItems = items.Result.SelectMany(i => i.Value).ToList();


		#region Mantis -> Board

		var ticketToCreate = new List<Ticket>();
		var ticketToUpdate = new List<(int IdWorkItem, Ticket Ticket)>();

		foreach (var ticket in tickets.Result)
		{
			var item = allItems.Find(i => i.IdMantis == ticket.IdMantis);
			if (item == default) ticketToCreate.Add(ticket);
			else if (item.MantisUpdatedAt > ticket.Dates.UpdatedAt) ticketToUpdate.Add((item.Id, ticket));
		}

		await ticketToCreate.Parallelize((t, _) => CreateWorkItem(t));
		await ticketToUpdate.Parallelize((pair, _) => UpdateWorkItem(pair.IdWorkItem, pair.Ticket));

		#endregion

		#region Board -> Mantis

		var boardToUpdate = allItems.Where(b => tickets.Result.First(t => t.IdMantis == b.IdMantis).Dates.UpdatedAt < b.UpdatedAt).ToList();

		await boardToUpdate.Parallelize((item, _) => UpdateTicket(item));

		#endregion
	}

	private Task UpdateTicket(Issue item)
	{
		using var _ = LogService($"{Log.F(item.IdMantis)}");

		return mantisClient.UpdateTicket(new UpdateTicketPayload
		{
			IdMantis = item.IdMantis,
			Priority = item.Priority,
			Severity = item.Severity,
			Status = item.Status
		});
	}

	private async Task<WorkItem> CreateWorkItem(Ticket t)
	{
		using var _ = LogService();

		return await devopsBoardClient.CreateWorkItem(new CreateWorkItemPayload
		{
			IdMantis = t.IdMantis,
			App = t.App,
			Comments = ComputeComments(t),
			Description = t.Description,
			Priority = t.Priority,
			Severity = t.Severity,
			Status = t.Status,
			Summary = t.Summary,
			MantisUpdatedAt = t.Dates.UpdatedAt ?? DateTime.Now,
			MantisCreatedAt = t.Dates.CreatedAt
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
			MantisUpdatedAt = t.Dates.UpdatedAt ?? DateTime.Now
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
			var cadenaChar = message.Private ? CadenaClosedChar : CadenaOpenChar;
			sb.Append($"<p>{cadenaChar} | {message.CreatedAt:g} | {message.Reporter}: {message.Text}</p>");
		}

		return $"<div>{sb}</div>";
	}
}