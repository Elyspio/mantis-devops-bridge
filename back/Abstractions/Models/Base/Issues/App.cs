using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;

namespace MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;

public class App
{
	public required AppName Name { get; set; }
	public required AppPlatform Platform { get; set; }
	public required string Environment { get; set; }
}