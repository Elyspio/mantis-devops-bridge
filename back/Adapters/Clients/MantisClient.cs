using Example.Api.Adapters.Rest.Assemblers;
using Example.Api.Adapters.Rest.Responses.Mantis;
using MantisDevopsBridge.Api.Abstractions.Configs;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Clients;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Tickets;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Example.Api.Adapters.Rest.Clients;

internal class MantisClient(IOptionsMonitor<MantisConfig> config, TicketAssembler ticketAssembler) : IMantisClient
{
	private EndpointElement Endpoint => config.CurrentValue.Endpoint;


	public async Task<List<Ticket>> GetAllTickets()
	{
		var tickets = new List<Ticket>();
		var page = 0;
		const int pageSize = 25;

		while (true)
		{
			var requestUri = $"{Endpoint.Host}/api/rest/issues?filter_id={config.CurrentValue.IdFilter}&page_size={pageSize}&page={page}";
			var data = await Get<GetTokenResponse>(requestUri);
			var ticketsAfterMinDate = data.Issues.Where(i => i.CreatedAt >= config.CurrentValue.MinIssuesDate).ToList();
			tickets.AddRange(ticketsAfterMinDate.Select(ticketAssembler.Convert));
			if (ticketsAfterMinDate.Count != data.Issues.Count) break;
			page++;
		}

		return tickets;
	}


	private async Task<T> Get<T>(string uri)
	{
		using var client = new HttpClient();
		var request = new HttpRequestMessage(HttpMethod.Get, uri);
		request.Headers.Add("Authorization", Endpoint.Token);
		var response = await client.SendAsync(request);
		response.EnsureSuccessStatusCode();
		var content = await response.Content.ReadAsStringAsync();
		return JsonConvert.DeserializeObject<T>(content)!;
	}
}