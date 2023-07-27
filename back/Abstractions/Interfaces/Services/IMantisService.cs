using MantisDevopsBridge.Api.Abstractions.Models.Base.Mantis.Tickets;

namespace MantisDevopsBridge.Api.Abstractions.Interfaces.Services;

public interface IMantisService
{
	Task<List<Ticket>> GetAll();
}