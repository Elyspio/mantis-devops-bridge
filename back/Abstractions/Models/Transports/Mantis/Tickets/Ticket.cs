using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;

namespace MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Tickets;

public class Ticket : Issue
{
	public required int Id { get; set; }
	public required List<IssueMessage> Messages { get; set; }
}