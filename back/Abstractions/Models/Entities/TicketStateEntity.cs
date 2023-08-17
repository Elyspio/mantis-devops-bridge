using MantisDevopsBridge.Api.Abstractions.Interfaces.Business;
using MantisDevopsBridge.Api.Abstractions.Models.Base.TicketState;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MantisDevopsBridge.Api.Abstractions.Models.Entities;

public sealed class TicketStateEntity : TicketStateBase, IEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public ObjectId Id { get; set; }
}