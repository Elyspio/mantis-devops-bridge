namespace MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;

public enum TicketStatus
{
	Unknown,
	Created,
	Feedback,
	Acknowledged,
	Confirmed,
	Assigned,
	Resolved,
	Delivered,
	DeliveredInQualif,
	DeliveredInPreProd,
	DeliveredProd,
	Closed
}