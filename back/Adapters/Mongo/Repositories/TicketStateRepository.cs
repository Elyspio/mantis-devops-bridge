using MantisDevopsBridge.Api.Abstractions.Common.Helpers;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Repositories;
using MantisDevopsBridge.Api.Abstractions.Models.Base.TicketState;
using MantisDevopsBridge.Api.Abstractions.Models.Entities;
using MantisDevopsBridge.Api.Adapters.Mongo.Repositories.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MantisDevopsBridge.Api.Adapters.Mongo.Repositories;

internal class TicketStateRepository : CrudRepository<TicketStateEntity, TicketStateBase>, ITicketStateRepository
{
	public TicketStateRepository(IConfiguration configuration, ILogger<BaseRepository<TicketStateEntity>> logger) : base(configuration, logger)
	{
		CreateIndexIfMissing(new[]
		{
			nameof(TicketStateEntity.IdMantis)
		}, true);
		CreateIndexIfMissing(new[]
		{
			nameof(TicketStateEntity.IdWorkItem)
		}, true);
	}


	public async Task<TicketStateEntity> Create(int idMantis, int idWorkItem, DateTime mantisSynchronizedAt, DateTime workItemSynchronizedAt)
	{
		using var logger = LogAdapter($"{Log.F(idMantis)} {Log.F(idWorkItem)}", autoExit: false);


		var existing = await EntityCollection.AsQueryable().FirstOrDefaultAsync(t => t.IdMantis == idMantis);

		if (existing is not null)
		{
			logger.Debug($"A State already exists for {Log.F(idMantis)} but for another WorkItem '{existing.IdWorkItem}' replacing it with '{idWorkItem}'");

			existing.IdWorkItem = idWorkItem;
			existing.MantisSynchronizedAt = mantisSynchronizedAt;
			existing.WorkItemSynchronizedAt = workItemSynchronizedAt;

			await EntityCollection.ReplaceOneAsync(t => t.Id == existing.Id, existing);

			return existing;
		}


		var entity = new TicketStateEntity
		{
			IdMantis = idMantis,
			IdWorkItem = idWorkItem,
			MantisSynchronizedAt = mantisSynchronizedAt,
			WorkItemSynchronizedAt = workItemSynchronizedAt
		};

		await EntityCollection.InsertOneAsync(entity);

		logger.Exit($"{Log.F(entity.Id.ToString())}");

		return entity;
	}

	public async Task UpdateState(int idMantis)
	{
		using var logger = LogAdapter($"{Log.F(idMantis)}", autoExit: false);

		var filter = Filter.Where(t => t.IdMantis == idMantis);

		var now = DateTime.Now;

		var update = Update
			.Set(t => t.WorkItemSynchronizedAt, now)
			.Set(t => t.MantisSynchronizedAt, now);

		var result = await EntityCollection.UpdateManyAsync(filter, update);


		logger.Exit($"{Log.F(result.MatchedCount)} {Log.F(result.ModifiedCount)}");
	}

	public async Task DeleteByIdWorkItem(int idWorkItem)
	{
		using var logger = LogAdapter($"{Log.F(idWorkItem)}", autoExit: false);

		var result = await EntityCollection.DeleteOneAsync(Filter.Where(t => t.IdWorkItem == idWorkItem));

		logger.Exit($"{Log.F(result.DeletedCount)}");
	}
}