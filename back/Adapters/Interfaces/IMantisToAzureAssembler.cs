using MantisDevopsBridge.Api.Adapters.Rest.Responses.Mantis;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace MantisDevopsBridge.Api.Adapters.Rest.Interfaces;

internal interface IMantisToAzureAssembler<T>
{
	T Convert(WorkItem wt);
	T Convert(Issue issue);

	string ToAzure(T elem);
	string ToMantis(T elem);
}