using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;
using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Tickets;
using MantisDevopsBridge.Api.Adapters.Rest.Assemblers.Properties;
using MantisDevopsBridge.Api.Adapters.Rest.Assemblers.Properties.App;
using MantisDevopsBridge.Api.Adapters.Rest.Responses.Mantis;
using Issue = MantisDevopsBridge.Api.Adapters.Rest.Responses.Mantis.Issue;

namespace MantisDevopsBridge.Api.Adapters.Rest.Assemblers;

internal sealed class TicketAssembler(PriorityAssembler priorityAssembler, SeverityAssembler severityAssembler, AppAssembler appAssembler)
{
	public Ticket Convert(Issue issue)
	{
		return new Ticket
		{
			IdMantis = issue.Id,
			Summary = issue.Summary,
			App = appAssembler.Convert(issue),
			Description = issue.Description,
			StepsToReproduce = issue.StepsToReproduce,
			Dates = new IssueDates
			{
				CreatedAt = issue.CreatedAt,
				UpdatedAt = issue.UpdatedAt
			},

			Severity = severityAssembler.Convert(issue),
			Priority = priorityAssembler.Convert(issue),
			Status = ParseStatus(issue.Status),
			Messages = issue.Notes?.Select(node => new IssueMessage
				           {
					           Reporter = node.Reporter.RealName,
					           Text = node.Text,
					           IdMessage = node.Id,
					           CreatedAt = node.CreatedAt,
					           Private = node.ViewState.Name == "private"
				           })
				           .ToList() ??
			           new List<IssueMessage>(),
			Users = new IssueUsers
			{
				Reporter = issue.Reporter.Email,
				Developer = issue.Handler?.Email
			}
		};
	}


	private TicketStatus ParseStatus(Status status)
	{
		return (TicketStatus)status.Id!.Value;
	}
}