using System.Text;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems;

namespace MantisDevopsBridge.Api.Core.Exceptions.WorkItems;

public class WorkItemDeleteException(Dictionary<WorkItem, Exception> exceptions) : Exception($"WorkItem Delete error: {exceptions.Aggregate(new StringBuilder(), (builder, pair) =>
{
	builder.AppendLine($"WorkItem: {pair} {pair.Value}");


	return builder;
})}");