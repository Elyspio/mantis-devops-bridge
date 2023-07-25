using MantisDevopsBridge.Api.Abstractions.Interfaces.Injections;
using Example.Api.Adapters.Rest.Configs;
using MantisDevopsBridge.Api.Abstractions.Common.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Api.Adapters.Rest.Injections;

public class RestAdapterModule : IDotnetModule
{
	public void Load(IServiceCollection services, IConfiguration configuration)
	{
		var config = configuration.GetRequiredSection(EndpointConfig.Section).Get<EndpointConfig>()!;

		Console.WriteLine(Log.F(config));
	}
}