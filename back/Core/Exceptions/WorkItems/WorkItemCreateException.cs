using System.Text;

namespace MantisDevopsBridge.Api.Core.Exceptions.WorkItems;

public class WorkItemCreateException(Dictionary<object, object> exceptions) : Exception($"WorkItem Create error: {exceptions.Aggregate(new StringBuilder(), (builder, pair) =>
{
	builder.AppendLine($"WorkItem: {pair} {pair.Value}");


	return builder;
})}");