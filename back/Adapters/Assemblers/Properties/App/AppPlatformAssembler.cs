using System.Diagnostics.CodeAnalysis;
using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;
using MantisDevopsBridge.Api.Adapters.Rest.Interfaces;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Issue = MantisDevopsBridge.Api.Adapters.Rest.Responses.Mantis.Issue;

namespace MantisDevopsBridge.Api.Adapters.Rest.Assemblers.Properties.App;

public sealed class AppPlatformAssembler : IMantisToAzureAssembler<List<AppPlatform>>
{
	private static readonly Dictionary<AppPlatform, string> _toAzureMapPlatforms = new()
	{
		{ AppPlatform.Bureau, "Bureau" },
		{ AppPlatform.Mobile, "Mobile" },
		{ AppPlatform.Web, "Web" },
	};

	private static readonly Dictionary<string, AppPlatform> _fromAzureMapPlatforms = _toAzureMapPlatforms.ToDictionary(pair => pair.Value, pair => pair.Key);

	public List<AppPlatform> Convert(WorkItem wt)
	{

		var tags = ((string)wt.Fields[WorkItemAssembler.TagsFieldId]).ToLower();
		return  _fromAzureMapPlatforms.Where(pair => tags.Contains(pair.Key.ToLower())).Select(pair => pair.Value).ToList();

	}

	public List<AppPlatform> Convert(Issue issue)
	{
		var category = issue.Category.Name.ToLower();

		AppPlatform? platform = null;
		if (category.Contains("bureau")) platform = AppPlatform.Bureau;
		if (category.Contains("mobile")) platform = AppPlatform.Mobile;
		if (category.Contains("web")) platform = AppPlatform.Web;

		if (platform is null)
			throw new ArgumentOutOfRangeException(nameof(category), $"La valeur {category} n'a pas pu être converti en {nameof(AppPlatform)}");

		return new List<AppPlatform> { platform.Value };
	}

	public string ToAzure(List<AppPlatform> platforms)
	{
		return string.Join(", ",  platforms.Select(p => _toAzureMapPlatforms[p]
		));
	}

	[DoesNotReturn]
	public string ToMantis(List<AppPlatform> elem)
	{
		throw new NotImplementedException("This method should not be called");
	}
}