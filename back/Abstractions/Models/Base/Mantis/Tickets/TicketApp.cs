using MantisDevopsBridge.Api.Abstractions.Models.Base.Mantis.Tickets.Enums;

namespace MantisDevopsBridge.Api.Abstractions.Models.Base.Mantis.Tickets;

public class TicketApp
{
	public required AppName Name { get; set; }
	public required AppPlatform Platform { get; set; }
	public required string Environment { get; set; }
}