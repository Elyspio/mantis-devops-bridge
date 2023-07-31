using System.Collections.Concurrent;
using Example.Api.Adapters.Rest.Assemblers;
using Example.Api.Adapters.Rest.Responses.Mantis;
using MantisDevopsBridge.Api.Abstractions.Common.Helpers;
using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Abstractions.Configs;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Clients;
using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.Payloads;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using WorkItem = MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems.WorkItem;

namespace Example.Api.Adapters.Rest.Clients;

public class DevopsBoardClient(ILogger<DevopsBoardClient> logger, IOptionsMonitor<DevopsConfig> configMonitor, WorkItemAssembler workItemAssembler) : TracingAdapter(logger),
	IDevopsBoardClient
{
	private DevopsConfig config => configMonitor.CurrentValue;
	private EndpointElement endpoint => config.Endpoint;

	public async Task<Dictionary<AppName, List<WorkItem>>> GetWorkItems()
	{
		using var _ = LogAdapter();

		var workItems = new ConcurrentBag<Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem>();

		var witClient = GetWorkItemClient();

		var appNames = Enum.GetValues<AppName>();

		foreach (var appName in appNames)
		{
			var area = configMonitor.CurrentValue.Areas[appName];
			var wiql = new Wiql { Query = $"select * from workitems where [System.AreaPath] = '{area}'" };

			// Execute the query to get work item IDs
			var queryResult = await witClient.QueryByWiqlAsync(wiql, configMonitor.CurrentValue.Project);

			// Fetch the work items using the IDs obtained from the query
			var workItemIds = queryResult.WorkItems.Select(wi => wi.Id);

			await Parallel.ForEachAsync(workItemIds, async (id, token) =>
			{
				var x = await witClient.GetWorkItemAsync(id, cancellationToken: token);
				workItems.Add(x);
			});
		}

		return workItems.Select(workItemAssembler.Convert).GroupBy(i => i.App.Name).ToDictionary(pair => pair.Key, pair => pair.ToList());
	}

	public async Task<WorkItem> CreateWorkItem(CreateWorkItemPayload workItem)
	{
		using var _ = LogAdapter();

		var client = GetWorkItemClient();

		var patchDocument = new JsonPatchDocument
		{
			AddField(WorkItemAssembler.AreaFieldId, configMonitor.CurrentValue.Areas[workItem.App.Name]),
			AddField(WorkItemAssembler.TitleFieldId, workItem.Summary),
			AddField(WorkItemAssembler.DescriptionFieldId, workItem.Description),
			AddField(WorkItemAssembler.CommentairesFieldId, workItem.Comments),
			AddField(WorkItemAssembler.TagsFieldId, workItemAssembler.ConvertPlatform(workItem.App.Platform)),
			AddField(WorkItemAssembler.SeverityFieldId, workItemAssembler.ConvertSeverity(workItem.Severity)),
			AddField(WorkItemAssembler.PriorityFieldId, workItemAssembler.ConvertPriority(workItem.Priority)),
			AddField(WorkItemAssembler.MantisIdField, workItem.IdMantis),
			AddField(WorkItemAssembler.MantisCreatedAtField, workItem.MantisCreatedAt),
			AddField(WorkItemAssembler.MantisUpdatedAtField, workItem.MantisUpdatedAt),
			AddField(WorkItemAssembler.HashField, workItem.Hash),
		};

		// Create the work item
		var result = await client.CreateWorkItemAsync(patchDocument, configMonitor.CurrentValue.Project, "Issue");

		var keys = workItemAssembler.GetStatusKey(result.Fields)!;

		patchDocument = new JsonPatchDocument
		{
			Capacity = keys.Count
		};

		patchDocument.AddRange(keys.Select(k => UpdateField(k, workItemAssembler.ConvertStatus(workItem.Status))));

		result = await client.UpdateWorkItemAsync(patchDocument, result.Id!.Value);

		return workItemAssembler.Convert(result);
	}

	public async Task<WorkItem> UpdateWorkItem(UpdateWorkItemPayload workItem)
	{
		using var _ = LogAdapter($"{Log.F(workItem.Id)}");

		var client = GetWorkItemClient();

		var realItem = await client.GetWorkItemAsync(workItem.Id);

		var keys = workItemAssembler.GetStatusKey(realItem.Fields)!;

		var patchDocument = new JsonPatchDocument
		{
			UpdateField(WorkItemAssembler.TitleFieldId, workItem.Summary),
			UpdateField(WorkItemAssembler.DescriptionFieldId, workItem.Description),
			UpdateField(WorkItemAssembler.CommentairesFieldId, workItem.Comments),
			UpdateField(WorkItemAssembler.SeverityFieldId, workItemAssembler.ConvertSeverity(workItem.Severity)),
			UpdateField(WorkItemAssembler.PriorityFieldId, workItemAssembler.ConvertPriority(workItem.Priority)),
			UpdateField(WorkItemAssembler.MantisUpdatedAtField, workItem.MantisUpdatedAt),
			UpdateField(WorkItemAssembler.UpdatedAtFieldId, workItem.MantisUpdatedAt),
			UpdateField(WorkItemAssembler.HashField, workItem.Hash),
		};

		patchDocument.AddRange(keys.Select(k => UpdateField(k, workItemAssembler.ConvertStatus(workItem.Status))));

		var result = await client.UpdateWorkItemAsync(patchDocument, workItem.Id);

		return workItemAssembler.Convert(result);
	}

	public async Task DeleteWorkItem(int id)
	{
		using var _ = LogAdapter($"{Log.F(id)}");

		var client = GetWorkItemClient();

		await client.DeleteWorkItemAsync(id);
	}


	private WorkItemTrackingHttpClient GetWorkItemClient()
	{
		// Create a connection to Azure DevOps
		var connection = new VssConnection(new Uri(endpoint.Host), new VssBasicCredential(string.Empty, endpoint.Token));

		// Get the work item tracking client
		return connection.GetClient<WorkItemTrackingHttpClient>();
	}


	private JsonPatchOperation AddField(string name, object value)
	{
		return new JsonPatchOperation
		{
			Value = value,
			Path = $"/fields/{name}",
			Operation = Operation.Add
		};
	}

	private JsonPatchOperation UpdateField(string name, object value)
	{
		return new JsonPatchOperation
		{
			Value = value,
			Path = $"/fields/{name}",
			Operation = Operation.Replace
		};
	}
}