using System.Text;
using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Abstractions.Configs;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Clients;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Services;
using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Tickets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MantisDevopsBridge.Api.Core.Services;

public class IssueService(IMantisClient mantisClient, IDevopsBoardClient devopsBoardClient, ILogger<IssueService> logger, IOptionsMonitor<MantisConfig> mantisConfig)
	: TracingService(logger), IIssueService
{
	public async Task<Dictionary<AppName, List<Ticket>>> GetAll()
	{
		using var _ = LogService();

		var tickets = mantisClient.GetAllTickets();

		var items = devopsBoardClient.GetWorkItems();


		await Task.WhenAll(tickets, items);


		var t = tickets.Result[0];

		await devopsBoardClient.CreateWorkItem(new CreateWorkItemPayload
		{
			App = t.App,
			Comments = ComputeComments(t),
			Dates = t.Dates,
			Description = t.Description,
			Priority = t.Priority,
			Severity = t.Severity,
			Status = t.Status,
			Summary = t.Summary
		});


		return new Dictionary<AppName, List<Ticket>>();

		// var tickets =  await mantisClient.GetAllTickets();
		//
		// return tickets.GroupBy(t => t.App.Name).ToDictionary(grp => grp.Key, grp => grp.ToList());
	}


	private string ComputeComments(Ticket ticket)
	{
		var sb = new StringBuilder();

		sb.Append($"""
		           <h1>Ticket Mantis n°
		           <a href="{mantisConfig.CurrentValue.Endpoint.Host}/view.php?id={ticket.Id}">{ticket.Id}</a>
		           </h1>
		           """);

		if (ticket.Messages.Count != 0) sb.Append(@"<h2>Messages</h2>");

		foreach (var message in ticket.Messages)
		{
			var cadenaChar = message.Private ? "🔒 " : "🔓 ";
			sb.Append($"<p>{cadenaChar} | {message.CreatedAt:g} | {message.Reporter}: {message.Text}</p>");
		}

		return $"<div>{sb}</div>";
	}
}