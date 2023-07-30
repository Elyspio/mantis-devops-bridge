using System.Text;
using Example.Api.Adapters.Rest.Assemblers;
using Example.Api.Adapters.Rest.Responses.Mantis;
using MantisDevopsBridge.Api.Abstractions.Common.Helpers;
using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Abstractions.Configs;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Clients;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Payloads;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Tickets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Example.Api.Adapters.Rest.Clients;

internal class MantisClient(ILogger<MantisConfig> logger, IOptionsMonitor<MantisConfig> config, TicketAssembler ticketAssembler) : TracingAdapter(logger), IMantisClient
{
	private EndpointElement Endpoint => config.CurrentValue.Endpoint;


	public async Task<List<Ticket>> GetAllTickets()
	{
		using var _ = LogAdapter();

		var tickets = new List<Ticket>();
		var page = 0;
		const int pageSize = 25;

		while (true)
		{
			var requestUri = $"{Endpoint.Host}/api/rest/issues?filter_id={config.CurrentValue.IdFilter}&page_size={pageSize}&page={page}";
			var data = await Request<GetTokenResponse>(HttpMethod.Get, requestUri);
			var ticketsAfterMinDate = data.Issues.Where(i => i.CreatedAt >= config.CurrentValue.MinIssuesDate).ToList();
			tickets.AddRange(ticketsAfterMinDate.Select(ticketAssembler.Convert));
			if (ticketsAfterMinDate.Count != data.Issues.Count) break;
			page++;
		}

		return tickets;
	}


	public async Task UpdateTicket(UpdateTicketPayload ticket)
	{
		using var _ = LogAdapter(Log.F(ticket.IdMantis));

		var requestUri = $"{Endpoint.Host}/api/rest/issues/{ticket.IdMantis}";

		await Request<object>(HttpMethod.Patch, requestUri, new
		{
			status = new
			{
				name = ticketAssembler.ConvertStatus(ticket.Status)
			},
			priority = new
			{
				name = ticketAssembler.ConvertPriority(ticket.Priority)
			},
			severity = new
			{
				name = ticketAssembler.ConvertSeverity(ticket.Severity)
			}
		});
	}


	private async Task<T> Request<T>(HttpMethod method, string uri, object? body = null)
	{
		using var _ = LogAdapter($"{method.Method} {uri}");
		using var client = new HttpClient();
		var request = new HttpRequestMessage(method, uri);
		request.Headers.Add("Authorization", Endpoint.Token);

		if (body is not null)
		{
			var bodyContent = JsonConvert.SerializeObject(body);
			request.Content = new StringContent(bodyContent, Encoding.UTF8, "application/json");
		}

		var response = await client.SendAsync(request);
		response.EnsureSuccessStatusCode();
		var content = await response.Content.ReadAsStringAsync();
		return JsonConvert.DeserializeObject<T>(content)!;
	}
}