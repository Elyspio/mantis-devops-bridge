using MantisDevopsBridge.Api.Abstractions.Models.Base.Mantis.Tickets.Enums;

namespace MantisDevopsBridge.Api.Abstractions.Models.Base.Mantis.Tickets;

public class Ticket
{
	// Identifier related to the issue
	public required int IdIssue { get; set; }

	// Brief description about the ticket
	public required string Summary { get; set; }

	// Application associated with this ticket
	public required TicketApp App { get; set; }

	// Indicator of ticket type: feature request (true) or bug (false)
	public required bool IsFeature { get; set; }

	// Collection of dates relevant to the ticket's lifecycle
	public required TicketDates Dates { get; set; }

	// Assessment of the problem's negative impact
	public required TicketSeverity Severity { get; set; }

	// Assessment of the problem's resolution urgency
	public required TicketPriority Priority { get; set; }

	// Current state of the ticket within the workflow
	public required TicketStatus Status { get; set; }

	// Collection of communication exchanges related to this ticket
	public required List<TicketMessage> Messages { get; set; }
}