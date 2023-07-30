﻿using System.Collections.Concurrent;
using Example.Api.Adapters.Rest.Assemblers;
using MantisDevopsBridge.Api.Abstractions.Configs;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Clients;
using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems;
using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using WorkItem = MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems.WorkItem;

namespace Example.Api.Adapters.Rest.Clients;

public class DevopsBoardClient(IOptionsMonitor<DevopsConfig> configMonitor, WorkItemAssembler workItemAssembler) : IDevopsBoardClient
{
	private DevopsConfig config => configMonitor.CurrentValue;
	private EndpointElement endpoint => config.Endpoint;

	public async Task<Dictionary<AppName, List<WorkItem>>> GetWorkItems()
	{
		var workItems = new ConcurrentBag<Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem>();


		var witClient = GetWorkItemClient();

		// var appNames = Enum.GetValues<AppName>();
		var appNames = new[] { AppName.Azurezo };


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

		// Create a WIQL (Work Item Query Language) query


		return workItems.Select(workItemAssembler.Convert).GroupBy(i => i.App.Name).ToDictionary(pair => pair.Key, pair => pair.ToList());
	}

	public async Task<WorkItem> CreateWorkItem(CreateWorkItemPayload workItem)
	{
		var client = GetWorkItemClient();

		var patchDocument = new JsonPatchDocument
		{
			AddField(WorkItemAssembler.AreaFieldId, configMonitor.CurrentValue.Areas[workItem.App.Name]),
			AddField(WorkItemAssembler.TitleFieldId, workItem.Summary),
			AddField(WorkItemAssembler.DescriptionFieldId, workItem.Description),
			AddField(WorkItemAssembler.CommentairesFieldId, workItem.Comments),
			AddField(WorkItemAssembler.TagsFieldId, workItemAssembler.ConvertPlatform(workItem.App.Platform)),
			AddField(WorkItemAssembler.SeverityFieldId, workItemAssembler.ConvertSeverity(workItem.Severity)),
			AddField(WorkItemAssembler.PriorityFieldId, workItemAssembler.ConvertPriority(workItem.Priority))
		};

		// Create the work item
		var result = await client.CreateWorkItemAsync(patchDocument, configMonitor.CurrentValue.Project, "Issue");

		patchDocument = new JsonPatchDocument
		{
			UpdateField(WorkItemAssembler.StatusFieldId, workItemAssembler.ConvertStatus(workItem.Status))
		};

		result = await client.UpdateWorkItemAsync(patchDocument, result.Id!.Value);


		return workItemAssembler.Convert(result);
	}


	private WorkItemTrackingHttpClient GetWorkItemClient()
	{
		// Create a connection to Azure DevOps
		var connection = new VssConnection(new Uri(endpoint.Host), new VssBasicCredential(string.Empty, endpoint.Token));

		// Get the work item tracking client
		return connection.GetClient<WorkItemTrackingHttpClient>();
	}


	private JsonPatchOperation AddField(string name, string value)
	{
		return new JsonPatchOperation
		{
			Value = value,
			Path = $"/fields/{name}",
			Operation = Operation.Add
		};
	}

	private JsonPatchOperation UpdateField(string name, string value)
	{
		return new JsonPatchOperation
		{
			Value = value,
			Path = $"/fields/{name}",
			Operation = Operation.Replace
		};
	}
}