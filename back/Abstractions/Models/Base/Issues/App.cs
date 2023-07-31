using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;

namespace MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;

public class App
{
	public required AppName Name { get; init; }
	public required AppPlatform Platform { get; init; }
	public required string Environment { get; init; }


	public new int GetHashCode()
	{
		return HashCode.Combine((int)Name, (int)Platform, Environment);
	}
}