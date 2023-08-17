namespace MantisDevopsBridge.Api.Abstractions.Models.Base.TicketState;

public abstract class TicketStateBase
{
	public required int IdMantis { get; init; }

	public required int IdWorkItem { get; set; }

	public required DateTime WorkItemSynchronizedAt { get; set; }

	public required DateTime MantisSynchronizedAt { get; set; }
}