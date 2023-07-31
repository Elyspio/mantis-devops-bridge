using Example.Api.Adapters.Rest.Responses.Mantis;
using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;
using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Tickets;
using Microsoft.Extensions.Logging;
using Issue = Example.Api.Adapters.Rest.Responses.Mantis.Issue;

namespace Example.Api.Adapters.Rest.Assemblers;

public class TicketAssembler(ILogger<TicketAssembler> logger)
{
	public Ticket Convert(Issue issue)
	{
		var fields = issue.CustomFields;

		return new Ticket
		{
			IdMantis = issue.Id,
			Summary = issue.Summary,
			App = new App
			{
				Name = ParseName(issue.Category.Name),
				Platform = ParsePlatform(issue.Category.Name),
				Environment = fields.FirstOrDefault(f => f.Field.Name == "environment")
					              ?.Value ??
				              "N/A"
			},
			Description = issue.Description,
			Dates = new IssueDates
			{
				CreatedAt = issue.CreatedAt,
				UpdatedAt = issue.UpdatedAt
			},
			Severity = ParseSeverity(issue.Severity),
			Priority = ParsePriority(issue.Priority),
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
			           new List<IssueMessage>()
		};
	}


	private TicketPriority ParsePriority(Priority priority)
	{
		return (TicketPriority)priority.Id!.Value;
	}

	private TicketStatus ParseStatus(Status status)
	{
		return (TicketStatus)status.Id!.Value;
	}

	private TicketSeverity ParseSeverity(Severity severity)
	{
		return (TicketSeverity)severity.Id!.Value;
	}

	private AppName ParseName(string category)
	{
		var content = category.ToLower();
		if (content.Contains("spico")) return AppName.Spico;
		if (content.Contains("azurezo")) return AppName.Azurezo;
		if (content.Contains("parceo")) return AppName.Parceo;

		throw new ArgumentOutOfRangeException(nameof(category), $"La valeur {category} n'a pas pu être converti en {nameof(AppName)}");
	}

	private AppPlatform ParsePlatform(string category)
	{
		var content = category.ToLower();
		if (content.Contains("bureau")) return AppPlatform.Bureau;
		if (content.Contains("mobile")) return AppPlatform.Mobile;
		if (content.Contains("web")) return AppPlatform.Web;

		throw new ArgumentOutOfRangeException(nameof(category), $"La valeur {category} n'a pas pu être converti en {nameof(AppPlatform)}");
	}
}