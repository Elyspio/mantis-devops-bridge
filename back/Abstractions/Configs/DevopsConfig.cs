namespace MantisDevopsBridge.Api.Abstractions.Configs;

public sealed class DevopsConfig
{
	public const string Section = "Devops";
	public required EndpointElement Endpoint { get; init; }

	public required string Project { get; init; }

	public required string Area { get; init; }
}