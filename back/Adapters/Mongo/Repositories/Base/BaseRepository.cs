using MantisDevopsBridge.Api.Abstractions.Common.Helpers;
using MantisDevopsBridge.Api.Abstractions.Common.Technical.Tracing;
using MantisDevopsBridge.Api.Adapters.Mongo.Technical;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace MantisDevopsBridge.Api.Adapters.Mongo.Repositories.Base;

/// <summary>
///     Manage entity in MongoDB
/// </summary>
/// <typeparam name="T">Entity implementation</typeparam>
public abstract class BaseRepository<T> : TracingAdapter
{
	private readonly string _collectionName;
	private readonly MongoContext _context;
	private readonly ILogger _logger;

	/// <summary>
	///     Default constructor
	/// </summary>
	/// <param name="configuration"></param>
	/// <param name="logger"></param>
	protected BaseRepository(IConfiguration configuration, ILogger logger) : base(logger)
	{
		_context = new MongoContext(configuration);
		_collectionName = typeof(T).Name[..^"Entity".Length];
		_logger = logger;
	}

	/// <summary>
	///     Implementation of the collection
	/// </summary>
	protected IMongoCollection<T> EntityCollection => _context.MongoDatabase.GetCollection<T>(_collectionName);


	/// <summary>
	///     Create an index for this collection
	/// </summary>
	/// <param name="properties"></param>
	/// <param name="unique"></param>
	protected void CreateIndexIfMissing(ICollection<string> properties, bool unique = false)
	{
		using var logger = LogAdapter($"{Log.F(properties)} {Log.F(unique)}", className: GetType().Name);


		var indexName = string.Join("-", properties);
		var indexes = EntityCollection.Indexes.List().ToList();
		var foundIndex = indexes.Any(index => index["name"] == indexName);

		var indexBuilder = Builders<T>.IndexKeys;

		var newIndex = indexBuilder.Combine(properties.Select(property => indexBuilder.Ascending(property)));


		var options = new CreateIndexOptions
		{
			Unique = unique,
			Name = indexName
		};

		var indexModel = new CreateIndexModel<T>(newIndex, options);


		if (foundIndex) return;

		logger.Warn($"Property {_collectionName}.{indexName} is not indexed, creating one");
		EntityCollection.Indexes.CreateOne(indexModel);
		logger.Warn($"Property {_collectionName}.{indexName} is now indexed");
	}
}