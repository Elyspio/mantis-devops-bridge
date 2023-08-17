using System.Collections.ObjectModel;
using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;
using MantisDevopsBridge.Api.Adapters.Rest.Interfaces;
using MantisDevopsBridge.Api.Adapters.Rest.Responses.Mantis;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Issue = MantisDevopsBridge.Api.Adapters.Rest.Responses.Mantis.Issue;

namespace MantisDevopsBridge.Api.Adapters.Rest.Assemblers.Properties;

public class MessageAssembler : IMantisToAzureAssembler<List<IssueMessage>>
{
	public List<IssueMessage> Convert(WorkItem wt)
	{
		throw new NotImplementedException();
	}

	public List<IssueMessage> Convert(Issue issue)
	{
		var notes = issue.Notes ?? ReadOnlyCollection<Note>.Empty;

		return notes.Select(node => new IssueMessage
			{
				Reporter = node.Reporter.RealName,
				Text = node.Text,
				IdMessage = node.Id,
				CreatedAt = node.CreatedAt,
				Private = node.ViewState.Name == "private"
			})
			.ToList();
	}

	public string ToAzure(List<IssueMessage> elem)
	{
		throw new NotImplementedException("This method is not implemented because it is not necessary.");
	}

	public string ToMantis(List<IssueMessage> elem)
	{
		throw new NotImplementedException("This method is not implemented because it is not necessary.");
	}
}