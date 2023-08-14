using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;

namespace MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;

public sealed class App
{
	public required AppRegion Region { get; init; }
	public required List<AppPlatform> Platforms { get; init; }
	public required string Environment { get; init; }


	public new int GetHashCode()
	{
		return HashCode.Combine((int)Region, Platforms, Environment);
	}
}