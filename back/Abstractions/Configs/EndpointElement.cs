namespace MantisDevopsBridge.Api.Abstractions.Configs;

public sealed class EndpointElement
{
	public required string Token { get; set; }
	public required string Host { get; set; }
}