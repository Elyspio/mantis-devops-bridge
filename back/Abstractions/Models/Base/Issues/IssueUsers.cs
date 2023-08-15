namespace MantisDevopsBridge.Api.Abstractions.Models.Base.Issues;

public sealed class IssueUsers
{
	/// <summary>
	/// Email of the user who created the issue
	/// </summary>
	public required string Reporter { get; init; }

	/// <summary>
	/// Email of the developer assigned to the issue
	/// </summary>
	public string? Developer { get; init; }

}