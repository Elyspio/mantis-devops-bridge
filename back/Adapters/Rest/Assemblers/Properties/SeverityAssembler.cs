using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;
using MantisDevopsBridge.Api.Adapters.Rest.Interfaces;
using MantisDevopsBridge.Api.Adapters.Rest.Responses.Mantis;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace MantisDevopsBridge.Api.Adapters.Rest.Assemblers.Properties;

public sealed class SeverityAssembler : IMantisToAzureAssembler<TicketSeverity>
{
	private static readonly Dictionary<TicketSeverity, string> _toAzureMap = new()
	{
		{ TicketSeverity.Block, "Bloquant" },
		{ TicketSeverity.Feature, "Fonctionnalité" },
		{ TicketSeverity.Minor, "Mineur" },
		{ TicketSeverity.Major, "Majeur" }
	};

	private static readonly Dictionary<string, TicketSeverity> _fromAzureMap = _toAzureMap.ToDictionary(pair => pair.Value, pair => pair.Key);

	public TicketSeverity Convert(WorkItem wt)
	{
		var priority = (string)wt.Fields[WorkItemAssembler.SeverityFieldId];
		return _fromAzureMap[priority];
	}

	public TicketSeverity Convert(Issue issue)
	{
		return (TicketSeverity)issue.Severity.Id!.Value;
	}

	public string ToAzure(TicketSeverity elem)
	{
		return _toAzureMap[elem];
	}

	public string ToMantis(TicketSeverity elem)
	{
		throw new NotImplementedException();
	}
}