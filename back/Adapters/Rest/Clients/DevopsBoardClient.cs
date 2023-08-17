using System.Collections.Concurrent;
using MantisDevopsBridge.Api.Abstractions.Common.Helpers;
using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Abstractions.Configs;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Clients;
using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.Payloads;
using MantisDevopsBridge.Api.Adapters.Rest.Assemblers;
using MantisDevopsBridge.Api.Adapters.Rest.Assemblers.Properties;
using MantisDevopsBridge.Api.Adapters.Rest.Assemblers.Properties.App;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using WorkItem = MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems.WorkItem;

namespace MantisDevopsBridge.Api.Adapters.Rest.Clients;

public sealed class DevopsBoardClient(ILogger<DevopsBoardClient> logger, IOptionsMonitor<DevopsConfig> configMonitor, WorkItemAssembler workItemAssembler, AppAssembler appAssembler, SeverityAssembler severityAssembler, PriorityAssembler priorityAssembler, StatusAssembler statusAssembler, AppRegionAssembler regionAssembler) : TracingAdapter(logger), IDevopsBoardClient
{
	private const string WorkItemType = "Bug Externe";
	private DevopsConfig config => configMonitor.CurrentValue;
	private EndpointElement endpoint => config.Endpoint;

	public async Task<Dictionary<AppRegion, List<WorkItem>>> GetWorkItems()
	{
		using var _ = LogAdapter();

		var workItems = new ConcurrentBag<Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem>();

		var wtClient = GetWorkItemClient();

		var area = configMonitor.CurrentValue.Area;
		var wiql = new Wiql { Query = $"select * from workitems where [System.AreaPath] = '{area}' and [System.WorkItemType] = '{WorkItemType}'" };

		// Execute the query to get work item IDs
		var queryResult = await wtClient.QueryByWiqlAsync(wiql, configMonitor.CurrentValue.Project);

		// Fetch the work items using the IDs obtained from the query
		var workItemIds = queryResult.WorkItems.Select(wi => wi.Id);

		await Parallel.ForEachAsync(workItemIds, async (id, token) => workItems.Add(await wtClient.GetWorkItemAsync(id, cancellationToken: token)));

		return workItems.Select(workItemAssembler.Convert).GroupBy(i => i.App.Region).ToDictionary(pair => pair.Key, pair => pair.ToList());
	}

	public async Task<WorkItem> CreateWorkItem(CreateWorkItemPayload workItem)
	{
		using var _ = LogAdapter();

		var client = GetWorkItemClient();

		var patchDocument = new JsonPatchDocument
		{
			AddField(WorkItemAssembler.AreaFieldId, configMonitor.CurrentValue.Area),
			AddField(WorkItemAssembler.TitleFieldId, workItem.Summary),
			AddField(WorkItemAssembler.DescriptionFieldId, workItem.Description),
			AddField(WorkItemAssembler.StepsToReproduceFieldId, workItem.StepsToReproduce),
			AddField(WorkItemAssembler.CommentairesFieldId, workItem.Comments),
			AddField(WorkItemAssembler.TagsFieldId, appAssembler.ComputeTags(workItem.App)),
			AddField(WorkItemAssembler.SeverityFieldId, severityAssembler.ToAzure(workItem.Severity)),
			AddField(WorkItemAssembler.PriorityFieldId, priorityAssembler.ToAzure(workItem.Priority)),
			AddField(WorkItemAssembler.RegionFieldId, regionAssembler.ToAzure(workItem.App.Region)),
			AddField(WorkItemAssembler.MantisIdField, workItem.IdMantis),
			AddField(WorkItemAssembler.MantisCreatedAtField, workItem.MantisCreatedAt),
			AddField(WorkItemAssembler.MantisUpdatedAtField, workItem.MantisUpdatedAt),
			AddField(WorkItemAssembler.HashField, workItem.Hash)
		};

		if (workItem.Users.Reporter.EndsWith("@coexya.eu")) patchDocument.Add(AddField(WorkItemAssembler.ReportedFieldId, workItem.Users.Reporter));

		if (workItem.Users.Developer?.EndsWith("@coexya.eu") == true) patchDocument.Add(AddField(WorkItemAssembler.DeveloperFieldId, workItem.Users.Developer));

		// Create the work item
		var result = await client.CreateWorkItemAsync(patchDocument, configMonitor.CurrentValue.Project, WorkItemType);

		var keys = statusAssembler.GetStatusKey(result.Fields)!;

		patchDocument = new JsonPatchDocument
		{
			Capacity = keys.Count
		};

		patchDocument.AddRange(keys.Select(k => UpdateField(k, statusAssembler.ToAzure(workItem.Status))));


		await Task.Delay(100);

		result = await client.UpdateWorkItemAsync(patchDocument, result.Id!.Value);

		return workItemAssembler.Convert(result);
	}

	public async Task<WorkItem> UpdateWorkItem(UpdateWorkItemPayload workItem)
	{
		using var _ = LogAdapter($"{Log.F(workItem.Id)}");

		var client = GetWorkItemClient();

		var realItem = await client.GetWorkItemAsync(workItem.Id);

		var keys = statusAssembler.GetStatusKey(realItem.Fields)!;

		var patchDocument = new JsonPatchDocument
		{
			UpdateField(WorkItemAssembler.TitleFieldId, workItem.Summary),
			UpdateField(WorkItemAssembler.DescriptionFieldId, workItem.Description),
			UpdateField(WorkItemAssembler.CommentairesFieldId, workItem.Comments),
			UpdateField(WorkItemAssembler.SeverityFieldId, severityAssembler.ToAzure(workItem.Severity)),
			UpdateField(WorkItemAssembler.PriorityFieldId, priorityAssembler.ToAzure(workItem.Priority)),
			UpdateField(WorkItemAssembler.MantisUpdatedAtField, workItem.MantisUpdatedAt),
			UpdateField(WorkItemAssembler.UpdatedAtFieldId, workItem.MantisUpdatedAt),
			UpdateField(WorkItemAssembler.HashField, workItem.Hash)
		};

		if (workItem.Users.Reporter.EndsWith("@coexya.eu")) patchDocument.Add(UpdateField(WorkItemAssembler.ReportedFieldId, workItem.Users.Reporter));


		if (workItem.Users.Developer?.EndsWith("@coexya.eu") == true) patchDocument.Add(AddField(WorkItemAssembler.DeveloperFieldId, workItem.Users.Developer));

		patchDocument.AddRange(keys.Select(k => UpdateField(k, statusAssembler.ToAzure(workItem.Status))));

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