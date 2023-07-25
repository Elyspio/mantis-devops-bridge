using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing.Base;
using MantisDevopsBridge.Api.Web.Technical.Helpers;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MantisDevopsBridge.Api.Web.Technical.Extensions;

/// <summary>
///     OpenTelemetry Extensions methods for <see cref="IServiceCollection" />
/// </summary>
public static class OpenTelemetryExtentions
{
	/// <summary>
	///     Activate open telemetry support
	/// </summary>
	/// <param name="services"></param>
	/// <param name="configuration"></param>
	/// <returns></returns>
	public static IServiceCollection AddAppOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
	{
		var sources = AssemblyHelper.GetClassWithInterface<Program, ITracingContext>().ToArray();

		services.AddOptions<OtlpExporterOptions>().Configure(opts => { opts.Endpoint = new Uri(configuration["OpenTelemetry:Url"]!); });

		services.AddOpenTelemetryEventLogging();

		services.AddOpenTelemetry()
			.ConfigureResource(conf => conf.AddService(configuration["OpenTelemetry:Service"]!))
			.WithTracing(tracingBuilder =>
			{
				tracingBuilder
					.SetErrorStatusOnException()
					.AddSource(sources)
					.AddSource("MongoDB.Driver.Core.Extensions.DiagnosticSources")
					// Configure adapter
					.AddAspNetCoreInstrumentation(o =>
					{
						o.Filter = ctx => ctx.Request.Path != "/metrics";
					})					
					.AddHttpClientInstrumentation(options => { options.RecordException = true; })
					// Configure exporters
					.AddOtlpExporter();
			}).WithMetrics(metricBuilder =>
			{
				metricBuilder
					// .AddMeter(sources)
					.AddRuntimeInstrumentation()
					.AddProcessInstrumentation()
					.AddHttpClientInstrumentation()
					.AddAspNetCoreInstrumentation(o =>
					{
						o.Filter = (_, ctx) => ctx.Request.Path != "/metrics";
					})
					.AddPrometheusExporter()
					.AddOtlpExporter();
			});


		return services;
	}
}