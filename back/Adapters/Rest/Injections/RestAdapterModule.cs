using MantisDevopsBridge.Api.Abstractions.Configs;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Injections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MantisDevopsBridge.Api.Adapters.Rest.Injections;

public sealed class RestAdapterModule : IDotnetModule
{
	public void Load(IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<MantisConfig>(config => configuration.GetSection(MantisConfig.Section).Bind(config));
		services.Configure<DevopsConfig>(config => configuration.GetSection(DevopsConfig.Section).Bind(config));

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