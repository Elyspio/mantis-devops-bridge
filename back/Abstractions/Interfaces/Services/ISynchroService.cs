namespace MantisDevopsBridge.Api.Abstractions.Interfaces.Services;

/// <summary>
///     Interface representing the service to handle synchronization operations between Azure DevOps board and Mantis bug
///     tracker.
/// </summary>
public interface ISynchroService
{
	/// <summary>
	///     Asynchronously synchronizes issue data between Azure DevOps board and Mantis bug tracker.
	/// </summary>
	Task Synchronize();
}