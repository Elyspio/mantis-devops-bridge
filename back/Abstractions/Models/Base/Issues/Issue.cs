using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;

namespace MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;

public class Issue
{
	public required string Summary { get; set; }
	public required string Description { get; set; }
	public required App App { get; set; }
	public required TicketSeverity Severity { get; set; }
	public required TicketPriority Priority { get; set; }
	public required TicketStatus Status { get; set; }

	public required IssueDates Dates { get; set; }
}