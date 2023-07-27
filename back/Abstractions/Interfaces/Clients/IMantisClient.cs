using MantisDevopsBridge.Api.Abstractions.Models.Base.Mantis.Tickets;

namespace MantisDevopsBridge.Api.Abstractions.Interfaces.Clients;

public interface IMantisClient
{
	Task<List<Ticket>> GetAllTickets();
}