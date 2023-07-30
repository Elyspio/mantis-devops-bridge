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
			Id = issue.Id,
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
		return priority.Name switch
		{
			"aucune" => TicketPriority.None,
			"none" => TicketPriority.None,
			"low" => TicketPriority.Low,
			"normal" => TicketPriority.Normal,
			"urgent" => TicketPriority.Urgent,
			"high" => TicketPriority.High,
			"immediate" => TicketPriority.Immediate,
			_ => TicketPriority.Unknown
		};
	}

	private TicketStatus ParseStatus(Status status)
	{
		return status.Name switch
		{
			"new" => TicketStatus.Created,
			"feedback" => TicketStatus.Feedback,
			"acknowledged" => TicketStatus.Acknowledged,
			"confirmed" => TicketStatus.Confirmed,
			"assigned" => TicketStatus.Assigned,
			"resolved" => TicketStatus.Resolved,
			"livre" => TicketStatus.Delivered,
			"livrerecette" => TicketStatus.DeliveredInQualif,
			"livrepreproduction" => TicketStatus.DeliveredInPreProd,
			"livreproduction" => TicketStatus.DeliveredProd,
			"closed" => TicketStatus.Closed,
			_ => TicketStatus.Unknown
		};
	}

	private TicketSeverity ParseSeverity(Severity severity)
	{
		var content = severity.Name.ToLower();
		if (content.Contains("bloquant")) return TicketSeverity.Block;
		if (content.Contains("majeur")) return TicketSeverity.Major;
		if (content.Contains("mineur")) return TicketSeverity.Minor;
		if (content.Contains("fonctionnalité")) return TicketSeverity.Feature;


		logger.LogWarning($"La valeur {severity.Name} n'a pas pu être converti en {nameof(TicketSeverity)}");

		return TicketSeverity.Unknown;
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