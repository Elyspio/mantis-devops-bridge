using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;
using MantisDevopsBridge.Api.Adapters.Rest.Interfaces;
using MantisDevopsBridge.Api.Adapters.Rest.Responses.Mantis;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace MantisDevopsBridge.Api.Adapters.Rest.Assemblers.Properties;

public class StatusAssembler : IMantisToAzureAssembler<TicketStatus>
{
	private static readonly Dictionary<TicketStatus, string> _toAzureMap = new()
	{
		{ TicketStatus.Created, "Nouveau" },
		{ TicketStatus.Acknowledged, "Accepté" },
		{ TicketStatus.Feedback, "Commentaire" },
		{ TicketStatus.Confirmed, "Confirmé" },
		{ TicketStatus.Assigned, "Affecté" },
		{ TicketStatus.Resolved, "Résolu" },
		{ TicketStatus.Delivered, "Livré Coexya" },
		{ TicketStatus.DeliveredInPreProd, "Livré Préprod" },
		{ TicketStatus.Closed, "Livré Fermé" }
	};

	private static readonly Dictionary<string, TicketStatus> _fromAzureMap = _toAzureMap.ToDictionary(pair => pair.Value, pair => pair.Key);


	public TicketStatus Convert(WorkItem wt)
	{
		var keys = GetStatusKey(wt.Fields);
		var statusKey = keys.Last();

		var statusValue = wt.Fields[statusKey] as string;

		return _fromAzureMap[statusValue];
	}

	public TicketStatus Convert(Issue issue)
	{
		return (TicketStatus)issue.Status.Id!.Value;
	}

	public string ToAzure(TicketStatus elem)
	{
		return _toAzureMap[elem];
	}

	public string ToMantis(TicketStatus elem)
	{
		throw new NotImplementedException("This method should not be called as TicketStatus ordinals are mantis's status ids");
	}

	public List<string> GetStatusKey(IDictionary<string, object> fields)
	{
		return fields.Keys.Where(k => k.EndsWith("Kanban.Column")).ToList();
	}
}