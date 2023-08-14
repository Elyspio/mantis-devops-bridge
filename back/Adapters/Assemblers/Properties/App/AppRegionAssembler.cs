using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;
using MantisDevopsBridge.Api.Adapters.Rest.Interfaces;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Issue = MantisDevopsBridge.Api.Adapters.Rest.Responses.Mantis.Issue;

namespace MantisDevopsBridge.Api.Adapters.Rest.Assemblers.Properties.App;

public sealed class AppRegionAssembler : IMantisToAzureAssembler<AppRegion>
{

	private static readonly Dictionary<AppRegion, string> _toAzureMapPlatforms = new()
	{
		{ AppRegion.Aura, "Aura" },
		{ AppRegion.Paca, "Paca" },
		{ AppRegion.Occitanie, "Occitanie" },
		{ AppRegion.Pulsy, "Pulsy" },
	};

	private static readonly Dictionary<string, AppRegion> _fromAzureMapPlatforms = _toAzureMapPlatforms.ToDictionary(pair => pair.Value, pair => pair.Key);


	public AppRegion Convert(WorkItem wt)
	{
		var region = (string)wt.Fields[WorkItemAssembler.RegionFieldId];
		return _fromAzureMapPlatforms[region];
	}

	public AppRegion Convert(Issue issue)
	{
		var category = issue.Category.Name.ToLower();

		AppRegion? region = null;
		if (category.Contains("spico")) region = AppRegion.Occitanie;
		if (category.Contains("azurezo")) region = AppRegion.Paca;
		if (category.Contains("parceo")) region = AppRegion.Pulsy;

		if (region is null)
			throw new ArgumentOutOfRangeException(nameof(issue.Category.Name), $"La valeur {category} n'a pas pu être converti en {nameof(AppRegion)}");

		return region.Value;
	}

	public string ToAzure(AppRegion elem)
	{
		return _toAzureMapPlatforms[elem];
	}

	public string ToMantis(AppRegion elem)
	{
		throw new NotImplementedException();
	}
}