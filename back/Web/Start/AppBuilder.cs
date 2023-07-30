using Example.Api.Adapters.Mongo.Injections;
using Example.Api.Adapters.Rest.Injections;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Injections;
using MantisDevopsBridge.Api.Core.Injections;
using MantisDevopsBridge.Api.Web.Technical.Extensions;

namespace MantisDevopsBridge.Api.Web.Start;

/// <summary>
///     Application builder
/// </summary>
public sealed class AppBuilder
{
	/// <summary>
	///     Create builder from command args
	/// </summary>
	/// <param name="args"></param>
	public AppBuilder(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		if (builder.Environment.IsDevelopment()) builder.Configuration.AddJsonFile("appsettings.secrets.json", true, true);
		builder.Configuration.AddJsonFile("appsettings.docker.json", true, true);

		builder.Services.AddModule<CoreModule>(builder.Configuration);
		builder.Services.AddModule<MongoAdapterModule>(builder.Configuration);
		builder.Services.AddModule<RestAdapterModule>(builder.Configuration);

		
		

		builder.Host.AddLogging();


		builder.Services
			.AddAppControllers()
			.AddAppSignalR()
			.AddAppSwagger();


		if (builder.Environment.IsDevelopment()) builder.Services.SetupDevelopmentCors();
		builder.Services.AddAppOpenTelemetry(builder.Configuration);

		Application = builder.Build();
	}

	/// <summary>
	///     Built application
	/// </summary>
	public WebApplication Application { get; }
}