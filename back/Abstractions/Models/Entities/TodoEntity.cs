using MantisDevopsBridge.Api.Abstractions.Interfaces.Business;
using MantisDevopsBridge.Api.Abstractions.Models.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MantisDevopsBridge.Api.Abstractions.Models.Entities;

public sealed class TodoEntity : TodoBase, IEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public ObjectId Id { get; set; }
}