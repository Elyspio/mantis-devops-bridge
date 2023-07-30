namespace MantisDevopsBridge.Api.Abstractions.Configs;

public class MantisConfig
{
	public const string Section = "Mantis";
	public required DateTime MinIssuesDate { get; set; }
	public required EndpointElement Endpoint { get; set; }
	public required int IdFilter { get; set; }
}