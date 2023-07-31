namespace MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;

public class IssueMessage
{
	public required int IdMessage { get; set; }
	public required string Text { get; init; }
	public required string Reporter { get; init; }
	public required DateTime CreatedAt { get; init; }
	public required bool Private { get; init; }
}