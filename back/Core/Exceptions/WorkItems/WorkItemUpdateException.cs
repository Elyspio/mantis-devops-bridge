using System.Text;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Tickets;

namespace MantisDevopsBridge.Api.Core.Exceptions.WorkItems;

public class WorkItemUpdateException(Dictionary<Ticket, Exception> exceptions) : Exception($"WorkItem Update error: {exceptions.Aggregate(new StringBuilder(), (builder, pair) =>
{
	builder.AppendLine($"WorkItem: {pair} {pair.Value}");


	return builder;
})}");