namespace MantisDevopsBridge.Api.Abstractions.Interfaces.Services;

/// <summary>
/// Interface for work items management.
/// </summary>
public interface IWorkItemService
{
	/// <summary>
	/// Deletes all work items.
	/// </summary>
	Task DeleteAllWorkItems();
}