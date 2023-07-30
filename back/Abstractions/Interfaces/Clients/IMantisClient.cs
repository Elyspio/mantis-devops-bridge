using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Payloads;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Tickets;

namespace MantisDevopsBridge.Api.Abstractions.Interfaces.Clients;

public interface IMantisClient
{
	Task<List<Ticket>> GetAllTickets();
	Task UpdateTicket(UpdateTicketPayload ticket);
}