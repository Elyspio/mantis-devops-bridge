using MantisDevopsBridge.Api.Abstractions.Models.Base.Issues.Enums;
using MantisDevopsBridge.Api.Abstractions.Models.Transports.Mantis.Tickets;

namespace MantisDevopsBridge.Api.Abstractions.Interfaces.Services;

/// <summary>
/// Interface representing the service to handle synchronization operations between Azure DevOps board and Mantis bug tracker.
/// </summary>
public interface IIssueService
{
	/// <summary>
	/// Asynchronously synchronizes issue data between Azure DevOps board and Mantis bug tracker.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation of synchronizing issues between Azure DevOps and Mantis.
	/// </returns>
	Task Synchronize();
}