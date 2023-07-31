namespace MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;

public enum TicketStatus
{
	Created = 10,
	Feedback = 20,
	Acknowledged = 30,
	Confirmed = 40,
	Assigned = 50,
	Resolved = 80,
	Delivered = 85,
	DeliveredInQualif = 86,
	DeliveredInPreProd = 87,
	DeliveredProd = 88,
	Closed = 90
}