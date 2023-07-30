using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;
using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Devops.WorkItems;
using Microsoft.Extensions.Logging;

namespace Example.Api.Adapters.Rest.Assemblers;

public class WorkItemAssembler(ILogger<WorkItemAssembler> logger)
{
	public const string SeverityFieldId = "Custom.3eb9946d-5bbf-4c69-97ee-7186896f0484";
	public const string PriorityFieldId = "Custom.047c51b8-74c0-47a8-aec9-03a0b341d6f8";
	public const string StatusFieldId = "WEF_69A555A049104BE7AB76C1579891E534_Kanban.Column";
	public const string TitleFieldId = "System.Title";
	public const string AreaFieldId = "System.AreaPath";
	public const string DescriptionFieldId = "System.Description";
	public const string TagsFieldId = "System.Tags";
	private const string CreatedAtFieldId = "System.CreatedDate";
	public const string UpdatedAtFieldId = "System.ChangedDate";
	public const string CommentairesFieldId = "Custom.Commentaires";
	public const string MantisIdField = "Custom.IdMantis";
	public const string MantisUpdatedAtField = "Custom.UpdatedAt";
	public const string MantisCreatedAtField = "Custom.CreatedAt";


	public WorkItem Convert(Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem item)
	{
		var fields = item.Fields;

		return new WorkItem
		{
			Summary = (string)fields[TitleFieldId],
			Id = item.Id!.Value,
			Description = (string)fields[DescriptionFieldId],
			App = new App
			{
				Environment = "N/A",
				Name = ParseName(fields),
				Platform = ParsePlatform(fields)
			},
			Severity = ParseSeverity(fields),
			Priority = ParsePriority(fields),
			Status = ParseStatus(fields),
			Comments = (string)fields[CommentairesFieldId],
			IdMantis = System.Convert.ToInt32(fields[MantisIdField]),
			MantisUpdatedAt = ParseDate(fields[MantisUpdatedAtField]),
			MantisCreatedAt = ParseDate(fields[MantisCreatedAtField]),
			CreatedAt = ParseDate(fields[CreatedAtFieldId]),
			UpdatedAt = ParseDate(fields[UpdatedAtFieldId])
		};
	}


	private TicketPriority ParsePriority(IDictionary<string, object> fields)
	{
		var priority = ((string)fields[PriorityFieldId]).ToLower();
		return priority switch
		{
			"aucune" => TicketPriority.None,
			"basse" => TicketPriority.Low,
			"normale" => TicketPriority.Normal,
			"élevée" => TicketPriority.High,
			"urgente" => TicketPriority.Urgent,
			"immédiate" => TicketPriority.Immediate,
			_ => TicketPriority.Unknown
		};
	}

	private TicketStatus ParseStatus(IDictionary<string, object> fields)
	{
		var status = ((string)fields[StatusFieldId]).ToLower();
		return status switch
		{
			"nouveau" => TicketStatus.Created,
			"commentaire" => TicketStatus.Feedback,
			"accepté" => TicketStatus.Acknowledged,
			"confirmé" => TicketStatus.Confirmed,
			"affecté" => TicketStatus.Assigned,
			"résolu" => TicketStatus.Resolved,
			"livré coexya" => TicketStatus.Delivered,
			"livre" => TicketStatus.DeliveredInQualif,
			"livré préprod" => TicketStatus.DeliveredInPreProd,
			"livré prod" => TicketStatus.DeliveredProd,
			"fermé" => TicketStatus.Closed,
			_ => TicketStatus.Unknown
		};
	}

	private TicketSeverity ParseSeverity(IDictionary<string, object> fields)
	{
		var severity = ((string)fields[SeverityFieldId]).ToLower();
		return severity switch
		{
			"fonctionnalité" => TicketSeverity.Feature,
			"mineur" => TicketSeverity.Minor,
			"majeur" => TicketSeverity.Major,
			"bloquant" => TicketSeverity.Block,
			_ => TicketSeverity.Unknown
		};
	}

	private AppName ParseName(IDictionary<string, object> fields)
	{
		var content = ((string)fields[AreaFieldId]).ToLower();
		if (content.Contains("spico")) return AppName.Spico;
		if (content.Contains("azurezo")) return AppName.Azurezo;
		if (content.Contains("parceo")) return AppName.Parceo;

		throw new ArgumentOutOfRangeException(nameof(content), $"La valeur {content} n'a pas pu être converti en {nameof(AppName)}");
	}

	private AppPlatform ParsePlatform(IDictionary<string, object> fields)
	{
		var tags = ((string)fields[TagsFieldId]).ToLower();
		if (tags.Contains("bureau")) return AppPlatform.Bureau;
		if (tags.Contains("mobile")) return AppPlatform.Mobile;
		if (tags.Contains("web")) return AppPlatform.Web;

		throw new ArgumentOutOfRangeException(nameof(tags), $"La valeur {tags} n'a pas pu être converti en {nameof(AppPlatform)}");
	}


	public string ConvertPlatform(AppPlatform platform)
	{
		return platform switch
		{
			AppPlatform.Mobile => "Mobile",
			AppPlatform.Web => "Web",
			AppPlatform.Bureau => "Bureau",
			_ => throw new ArgumentOutOfRangeException(nameof(platform), platform, null)
		};
	}

	public string ConvertSeverity(TicketSeverity workItemSeverity)
	{
		return workItemSeverity switch
		{
			TicketSeverity.Unknown => "Inconnu",
			TicketSeverity.Feature => "Fonctionnalité",
			TicketSeverity.Minor => "Mineur",
			TicketSeverity.Major => "Majeur",
			TicketSeverity.Block => "Bloquant",
			_ => throw new ArgumentOutOfRangeException(nameof(workItemSeverity), workItemSeverity, null)
		};
	}

	public string ConvertPriority(TicketPriority workItemPriority)
	{
		return workItemPriority switch
		{
			TicketPriority.Unknown => "inconnue",
			TicketPriority.None => "aucune",
			TicketPriority.Low => "basse",
			TicketPriority.Normal => "normale",
			TicketPriority.High => "élevée",
			TicketPriority.Urgent => "urgente",
			TicketPriority.Immediate => "immédiate",
			_ => throw new ArgumentOutOfRangeException(nameof(workItemPriority), workItemPriority, null)
		};
	}

	public string ConvertStatus(TicketStatus workItemStatus)
	{
		return workItemStatus switch
		{
			TicketStatus.Unknown => "Nouveau",
			TicketStatus.Created => "Nouveau",
			TicketStatus.Feedback => "Commentaire",
			TicketStatus.Acknowledged => "Accepté",
			TicketStatus.Confirmed => "Confirmé",
			TicketStatus.Assigned => "Affecté",
			TicketStatus.Resolved => "Résolu",
			TicketStatus.Delivered => "Livré Coexya",
			TicketStatus.DeliveredInQualif => "Livré Qualif",
			TicketStatus.DeliveredInPreProd => "Livré Préprod",
			TicketStatus.DeliveredProd => "Livré Prod",
			TicketStatus.Closed => "Fermé",
			_ => throw new ArgumentOutOfRangeException(nameof(workItemStatus), workItemStatus, null)
		};
	}

	private DateTime ParseDate(object date)
	{
		return ((DateTime)date).ToLocalTime();
	}
}