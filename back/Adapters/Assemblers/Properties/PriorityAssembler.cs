using MantisDevopsBridge.Api.Adapters.Rest.Interfaces;
using MantisDevopsBridge.Api.Adapters.Rest.Responses.Mantis;
using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace MantisDevopsBridge.Api.Adapters.Rest.Assemblers.Properties;

public sealed class PriorityAssembler : IMantisToAzureAssembler<TicketPriority>
{
	private static readonly Dictionary<TicketPriority, string> _toAzureMap = new()
	{
		{ TicketPriority.None, "0-Aucune" },
		{ TicketPriority.Low, "1-Basse" },
		{ TicketPriority.Normal, "2-Normale" },
		{ TicketPriority.High, "3-Élevée" },
		{ TicketPriority.Urgent, "4-Urgente" },
		{ TicketPriority.Immediate, "5-Immédiate" }
	};

	private static readonly Dictionary<string, TicketPriority> _fromAzureMap = _toAzureMap.ToDictionary(pair => pair.Value, pair => pair.Key);

	public TicketPriority Convert(WorkItem wt)
	{
		var priority = (string)wt.Fields[WorkItemAssembler.PriorityFieldId];
		return _fromAzureMap[priority];
	}

	public TicketPriority Convert(Issue issue)
	{
		return (TicketPriority)issue.Priority.Id!.Value;
	}

	public string ToAzure(TicketPriority elem)
	{
		return _toAzureMap[elem];
	}

	public string ToMantis(TicketPriority elem)
	{
		throw new NotImplementedException();
	}
}