using MantisDevopsBridge.Api.Web.Technical.Extensions;

namespace MantisDevopsBridge.Api.Web.Start;

/// <summary>
///     Application Initializer
/// </summary>
public static class AppRuntime
{
	/// <summary>
	///     Initialize runtime middlewares
	/// </summary>
	/// <param name="app"></param>
	/// <returns></returns>
	public static WebApplication Initialize(this WebApplication app)
	{
		// Allow CORS
		app.UseCors();

		app.UseAppSwagger();


		app.UseOpenTelemetryPrometheusScrapingEndpoint();

		// Setup Controllers
		app.MapControllers();

		if (!app.Environment.IsProduction()) return app;

		// Start SPA serving
		app.UseRouting();

		// Setup authentication
		app.UseAuthentication();
		app.UseAuthorization();

		app.UseStaticFiles();

		app.MapWhen(ctx => !ctx.Request.Path.StartsWithSegments("/api"), appBuilder =>
		{
			appBuilder.UseRouting();
			appBuilder.UseEndpoints(ep => { ep.MapFallbackToFile("index.html"); });
		});

		return app;
	}
}