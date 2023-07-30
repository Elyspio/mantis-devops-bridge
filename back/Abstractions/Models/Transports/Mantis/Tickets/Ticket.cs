using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;

namespace MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Tickets;

public class Ticket : Issue
{
	public required List<IssueMessage> Messages { get; set; }

	public required IssueDates Dates { get; set; }
}