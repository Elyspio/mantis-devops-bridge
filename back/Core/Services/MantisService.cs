using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Clients;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Services;
using MantisDevopsBridge.Api.Abstractions.Models.Base.Mantis.Tickets;
using Microsoft.Extensions.Logging;

namespace MantisDevopsBridge.Api.Core.Services;

public class MantisService(IMantisClient mantisClient, ILogger<MantisService> logger) : TracingService(logger), IMantisService
{
	public async Task<List<Ticket>> GetAll()
	{
		using var _ = LogService();

		return await mantisClient.GetAllTickets();
	}
}