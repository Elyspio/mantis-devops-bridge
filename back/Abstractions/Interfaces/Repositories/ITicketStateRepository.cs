using MantisDevopsBridge.Api.Abstractions.Models.Base.TicketState;
using MantisDevopsBridge.Api.Abstractions.Models.Entities;

namespace MantisDevopsBridge.Api.Abstractions.Interfaces.Repositories;

public interface ITicketStateRepository : ICrudRepository<TicketStateEntity, TicketStateBase>
{
	/// <summary>
	///     Create a new TicketState
	/// </summary>
	/// <param name="idMantis"></param>
	/// <param name="idWorkItem"></param>
	/// <param name="mantisSynchronizedAt"></param>
	/// <param name="workItemSynchronizedAt"></param>
	/// <returns></returns>
	Task<TicketStateEntity> Create(int idMantis, int idWorkItem, DateTime mantisSynchronizedAt, DateTime workItemSynchronizedAt);


	/// <summary>
	///     Update the synchro time of a TicketState
	/// </summary>
	/// <param name="idMantis"></param>
	/// <returns></returns>
	Task UpdateState(int idMantis);

	/// <summary>
	///     Delete a TicketState by IdWorkItem
	/// </summary>
	/// <param name="idWorkItem"></param>
	/// <returns></returns>
	Task DeleteByIdWorkItem(int idWorkItem);
}