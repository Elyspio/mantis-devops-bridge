﻿using MantisDevopsBridge.Api.Abstractions.Interfaces.Injections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MantisDevopsBridge.Api.Adapters.Mongo.Injections;

public sealed class MongoAdapterModule : IDotnetModule
{
	public void Load(IServiceCollection services, IConfiguration configuration)
	{
		var nsp = typeof(MongoAdapterModule).Namespace!;
		var baseNamespace = nsp[..nsp.LastIndexOf(".", StringComparison.Ordinal)];
		services.Scan(scan => scan
			.FromAssemblyOf<MongoAdapterModule>()
			.AddClasses(classes => classes.InNamespaces(baseNamespace + ".Repositories"))
			.AsImplementedInterfaces()
			.WithSingletonLifetime()
		);
	}
}