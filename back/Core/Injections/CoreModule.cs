using MantisDevopsBridge.Api.Abstractions.Interfaces.Injections;
using MantisDevopsBridge.Api.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MantisDevopsBridge.Api.Core.Injections;

public sealed class CoreModule : IDotnetModule
{
	public void Load(IServiceCollection services, IConfiguration configuration)
	{
		var nsp = typeof(CoreModule).Namespace!;
		var baseNamespace = nsp[..nsp.LastIndexOf(".", StringComparison.Ordinal)];
		services.Scan(scan => scan
			.FromAssemblyOf<CoreModule>()
			.AddClasses(classes => classes.InNamespaces(baseNamespace + ".Services"))
			.AsImplementedInterfaces()
			.WithSingletonLifetime()
		);

		services.AddHostedService<BridgeService>();
	}
}