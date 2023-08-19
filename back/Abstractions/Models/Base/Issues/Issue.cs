using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;

namespace MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;

public class Issue
{
	public required int IdMantis { get; init; }
	public required string Summary { get; init; }
	public required string Description { get; init; }
	public required string? StepsToReproduce { get; init; }
	public required App App { get; init; }
	public required TicketSeverity Severity { get; init; }
	public required TicketPriority Priority { get; init; }
	public required TicketStatus Status { get; init; }
	public required IssueUsers Users { get; init; }


	public override string ToString()
	{
		return $"IdMantis: {IdMantis}, Summary: {Summary}, Description: {Description}, StepsToReproduce: {StepsToReproduce}, App: ({App}), Severity: {Severity}, Priority: {Priority}, Status: {Status}, Users: ({Users})";
	}
}