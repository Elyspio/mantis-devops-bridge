namespace MantisDevopsBridge.Api.Abstractions.Models.Base.Mantis.Tickets.Enums;

public enum TicketStatus
{
	Unknown,
	Created,
	Feedback,
	Acknowledged,
	Confirmed,
	Assigned,
	Resolved,
	Deployed,
	DeployedRecette,
	DeployedPreProd,
	DeployedProd,
	Closed
}