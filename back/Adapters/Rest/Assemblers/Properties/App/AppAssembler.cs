using System.Diagnostics.CodeAnalysis;
using MantisDevopsBridge.Api.Adapters.Rest.Interfaces;
using MantisDevopsBridge.Api.Adapters.Rest.Responses.Mantis;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace MantisDevopsBridge.Api.Adapters.Rest.Assemblers.Properties.App;

public class AppAssembler(AppPlatformAssembler platformAssembler, AppRegionAssembler regionAssembler) : IMantisToAzureAssembler<Abstractions.Models.Base.Issues.App>
{
	public Abstractions.Models.Base.Issues.App Convert(WorkItem wt)
	{
		return new Abstractions.Models.Base.Issues.App
		{
			Platforms = platformAssembler.Convert(wt),
			Region = regionAssembler.Convert(wt),
			Environment = "N/A"
		};
	}

	public Abstractions.Models.Base.Issues.App Convert(Issue issue)
	{
		var envField = issue.CustomFields.FirstOrDefault(f => f.Field.Name == "environment");
		return new Abstractions.Models.Base.Issues.App
		{
			Platforms = platformAssembler.Convert(issue),
			Region = regionAssembler.Convert(issue),
			Environment = envField?.Value ?? "N/A"
		};
	}

	[DoesNotReturn]
	public string ToAzure(Abstractions.Models.Base.Issues.App elem)
	{
		throw new NotImplementedException("This method should not be called");
	}

	[DoesNotReturn]
	public string ToMantis(Abstractions.Models.Base.Issues.App elem)
	{
		throw new NotImplementedException("This method should not be called");
	}


	public string ComputeTags(Abstractions.Models.Base.Issues.App app)
	{
		return string.Join(", ", app.Platforms);
	}
}