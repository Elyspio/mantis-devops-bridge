using System.ComponentModel.DataAnnotations;
using MantisDevopsBridge.Api.Abstractions.Interfaces.Business;
using MantisDevopsBridge.Api.Abstractions.Models.Base.TicketState;

namespace MantisDevopsBridge.Api.Abstractions.Models.Transports;

public sealed class TicketState : TicketStateBase, ITransport
{
	[Required] public required Guid Id { get; init; }
}