using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Tickets;

namespace MantisDevopsBridge.Api.Abstractions.Interfaces.Services;

public interface IIssueService
{
	Task<Dictionary<AppName, List<Ticket>>> GetAll();
}