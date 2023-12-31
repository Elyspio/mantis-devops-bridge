﻿using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems;
using MantisDevopsBridge.Api.Adapters.Rest.Assemblers.Properties;
using MantisDevopsBridge.Api.Adapters.Rest.Assemblers.Properties.App;
using Microsoft.VisualStudio.Services.WebApi;

namespace MantisDevopsBridge.Api.Adapters.Rest.Assemblers;

public sealed class WorkItemAssembler(AppAssembler appAssembler, PriorityAssembler priorityAssembler, SeverityAssembler severityAssembler, StatusAssembler statusAssembler)
{
	public const string SeverityFieldId = "Custom.3eb9946d-5bbf-4c69-97ee-7186896f0484";
	public const string RegionFieldId = "Custom.972d0f37-c1aa-45d5-b4e2-e206b407f08a";
	public const string PriorityFieldId = "Custom.5a605c5b-ace5-4fb4-a934-2960ddb2940a";

	public const string DeveloperFieldId = "Custom.8ac1102b-d901-4ec5-ba01-d0407b2cccb5";
	public const string ReportedFieldId = "Custom.Reporteur";

	public const string TitleFieldId = "System.Title";
	public const string AreaFieldId = "System.AreaPath";
	public const string DescriptionFieldId = "System.Description";
	public const string TagsFieldId = "System.Tags";
	private const string CreatedAtFieldId = "System.CreatedDate";
	public const string UpdatedAtFieldId = "System.ChangedDate";
	public const string CommentairesFieldId = "Custom.Commentaires";
	public const string StepsToReproduceFieldId = "Custom.StepsToReproduce";
	public const string MantisIdField = "Custom.IdMantis";
	public const string MantisUpdatedAtField = "Custom.UpdatedAt";
	public const string MantisCreatedAtField = "Custom.CreatedAt";
	public const string HashField = "Custom.Hash";


	public WorkItem Convert(Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem item)
	{
		var fields = item.Fields;

		return new WorkItem
		{
			Id = item.Id!.Value,
			Summary = (string)fields[TitleFieldId],
			Description = TryGetField(fields, DescriptionFieldId, ""),
			StepsToReproduce = TryGetField(fields, StepsToReproduceFieldId, ""),
			App = appAssembler.Convert(item),
			Severity = severityAssembler.Convert(item),
			Priority = priorityAssembler.Convert(item),
			Status = statusAssembler.Convert(item),
			Comments = TryGetField(fields, CommentairesFieldId, ""),
			IdMantis = System.Convert.ToInt32(fields[MantisIdField]),
			CreatedAt = ParseDate(fields[CreatedAtFieldId]),
			UpdatedAt = ParseDate(fields[UpdatedAtFieldId]),
			Users = new IssueUsers
			{
				Reporter = GetEmailFromIdentityRef(fields, ReportedFieldId)!,
				Developer = GetEmailFromIdentityRef(fields, DeveloperFieldId)
			}
		};
	}


	private T TryGetField<T>(IDictionary<string, object>? fields, string field, T defaultValue)
	{
		var found = fields.TryGetValue(field, out var val);

		return !found ? defaultValue : (T)val!;
	}


	private string? GetEmailFromIdentityRef(IDictionary<string, object>? fields, string field)
	{
		var found = fields.TryGetValue(field, out var val);

		return !found ? null : ((IdentityRef)val!).UniqueName;
	}


	private DateTime ParseDate(object date)
	{
		return ((DateTime)date).ToLocalTime();
	}
}