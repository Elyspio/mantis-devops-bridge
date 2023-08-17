namespace MantisDevopsBridge.Api.Abstractions.Interfaces.Services;

public interface ITicketStateService
{
	/// <summary>
	///     Regenerate all TicketState from Mantis and Devops (both tickets have to exist)
	/// </summary>
	/// <returns></returns>
	Task Regenerate();
}