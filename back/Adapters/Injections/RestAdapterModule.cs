using Example.Api.Adapters.Rest.Configs;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Injections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Api.Adapters.Rest.Injections;

public class RestAdapterModule : IDotnetModule
{
	public void Load(IServiceCollection services, IConfiguration configuration)
	{
		var config = configuration.GetRequiredSection(EndpointConfig.Section).Get<EndpointConfig>()!;


		services.AddSingleton(config);

		var nsp = typeof(RestAdapterModule).Namespace!;
		var baseNamespace = nsp[..nsp.LastIndexOf(".", StringComparison.Ordinal)];

		services.Scan(scan => scan
			.FromAssemblyOf<RestAdapterModule>()
			.AddClasses(classes => classes.InNamespaces(baseNamespace + ".Clients"))
			.AsImplementedInterfaces()
			.WithSingletonLifetime()
		);

		services.Scan(scan => scan
			.FromAssemblyOf<RestAdapterModule>()
			.AddClasses(classes => classes.InNamespaces(baseNamespace + ".Assemblers"))
			.AsSelf()
			.WithSingletonLifetime()
		);
	}
}