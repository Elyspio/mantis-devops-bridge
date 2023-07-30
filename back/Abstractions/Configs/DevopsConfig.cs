using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;

namespace MantisDevopsBridge.Api.Abstractions.Configs;

public class DevopsConfig
{
	public const string Section = "Devops";
	public required EndpointElement Endpoint { get; set; }

	public required string Project { get; set; }

	public required Dictionary<AppName, string> Areas { get; set; }
}