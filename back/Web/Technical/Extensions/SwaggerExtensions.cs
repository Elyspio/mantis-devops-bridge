using MantisDevopsBridge.Api.Web.Technical.Filters.Swagger;
using Microsoft.OpenApi.Models;

namespace MantisDevopsBridge.Api.Web.Technical.Extensions;

/// <summary>
///     Swagger Extensions methods for <see cref="IServiceCollection" />
/// </summary>
public static class SwaggerExtentions
{
	/// <summary>
	///     Activate swagger support
	/// </summary>
	/// <param name="services"></param>
	/// <returns></returns>
	public static IServiceCollection AddAppSwagger(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddEndpointsApiExplorer();

		var xmlPaths = Directory.GetFiles(AppContext.BaseDirectory).ToList().Where(f => f.EndsWith(".xml"));

		services.AddSwaggerGen(options =>
		{
			options.SupportNonNullableReferenceTypes();
			options.OperationFilter<SwaggerSetNullableOperationFilter>();
			options.OperationFilter<SwaggerRemoveVersionFilter>();
			options.SchemaFilter<NullableSchemaFilter>();

			options.UseAllOfToExtendReferenceSchemas();
			options.UseAllOfForInheritance();
			options.CustomOperationIds(e => e.ActionDescriptor.RouteValues["action"]);


			foreach (var xmlPath in xmlPaths) options.IncludeXmlComments(xmlPath);


			var idTenant = configuration["AzureAd:TenantId"];
			options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
			{
				Type = SecuritySchemeType.OAuth2,
				Flows = new OpenApiOAuthFlows
				{
					Implicit = new OpenApiOAuthFlow
					{
						AuthorizationUrl = new Uri($"https://login.microsoftonline.com/{idTenant}/oauth2/v2.0/authorize"),
						TokenUrl = new Uri($"https://login.microsoftonline.com/{idTenant}/oauth2/v2.0/token"),
						Scopes = new Dictionary<string, string>
						{
							{
								"api://18622d82-eead-445d-9c2a-dffbc3e09078/MantisDevopsBridge",
								"Reads user's information"
							}
						}
					}
				}
			});

			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "oauth2"
						},
						Scheme = "oauth2",
						Name = "oauth2",
						In = ParameterLocation.Header
					},
					new List<string>()
				}
			});
		}).AddSwaggerGenNewtonsoftSupport();

		return services;
	}

	/// <summary>
	///     Active la gestion de swagger et son interface en gérant le versioning
	/// </summary>
	/// <param name="app"></param>
	/// <returns></returns>
	public static WebApplication UseAppSwagger(this WebApplication app)
	{
		app.UseSwagger(options =>
		{
			options.PreSerializeFilters.Add((document, request) =>
			{
				var uri = request.Headers.Referer.FirstOrDefault()!;
				document.Servers = new List<OpenApiServer>
				{
					new()
					{
						Url = uri[..uri.IndexOf("swagger/", StringComparison.Ordinal)]
					}
				};
			});
		});

		var clientId = app.Configuration["AzureAd:ClientId"];
		app.UseSwaggerUI(c =>
		{
			c.OAuthClientId(clientId);
			c.OAuthClientSecret("n.J8Q~38F8QwalRvcTgnt4NFQGGVauI02eWuQb2s");
			c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
		});

		return app;
	}
}