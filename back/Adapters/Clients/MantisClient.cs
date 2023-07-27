using Example.Api.Adapters.Rest.Assemblers;
using Example.Api.Adapters.Rest.Configs;
using Example.Api.Adapters.Rest.Responses.Mantis;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Clients;
using MantisDevopsBridge.Api.Abstractions.Models.Base.Mantis.Tickets;
using Newtonsoft.Json;

namespace Example.Api.Adapters.Rest.Clients;

internal class MantisClient(EndpointConfig endpointConfig, TicketAssembler ticketAssembler) : IMantisClient
{
	private const int IdFilter = 10952;

	public async Task<List<Ticket>> GetAllTickets()
	{
		using var client = new HttpClient();
		var request = new HttpRequestMessage(HttpMethod.Get, $"{endpointConfig.Mantis.Host}/api/rest/issues?filter_id={IdFilter}&page_size=99999&page=0");
		request.Headers.Add("Authorization", endpointConfig.Mantis.Token);
		var response = await client.SendAsync(request);
		response.EnsureSuccessStatusCode();
		var content = await response.Content.ReadAsStringAsync();
		var data = JsonConvert.DeserializeObject<GetTokenResponse>(content);


		return data!.Issues.Select(ticketAssembler.Convert).ToList();
	}
}